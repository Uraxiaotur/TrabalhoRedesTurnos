using UnityEngine;
using UnityEngine;
using TMPro;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.UI;

public class Network : MonoBehaviour
{
    public static Network Instance;
    
    [Header("Rede")]
    public bool isServer; //True para a máquina que fica "esperando", false para a outra
    public string serverIP = "127.0.0.1"; //IP da máquina que foi ligada primeiro
    public int port = 7777;

    [Header("Cena")]
    public TMP_Text logText;
    public List<Button> pecasBrancas;
    public List<Button> pecasPretas;


    TcpListener listener;
    TcpClient client;
    NetworkStream stream;

    Thread networkThread;
    Thread receiveThread;

    Queue<string> messages = new Queue<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Subscribe to the string-based message event
        GameOM.OnSendCheckerPositionMessageString += SendPosition;
    }

    void OnDisable()
    {
        GameOM.OnSendCheckerPositionMessageString -= SendPosition;
    }
    
    void Start()
    {
       if(isServer)
       {
           networkThread = new Thread(StartServer);

            networkThread.Start();
        }
        else
        {
            networkThread =
                new Thread(StartClient);

            networkThread.Start();
        }
    }

    void StartServer()
    {
        AddMessage("Servidor iniciado");

        listener =
            new TcpListener(
                IPAddress.Any,
                port);

        listener.Start();

        AddMessage("Esperando conexão...");

        client =
            listener.AcceptTcpClient();

        stream =
            client.GetStream();

        AddMessage("Cliente conectado");

        StartReceiveThread();
    }

    void StartClient()
    {
        AddMessage("Conectando...");

        client =
            new TcpClient();

        client.Connect(
            serverIP,
            port);

        stream =
            client.GetStream();

        AddMessage("Conectado");

        StartReceiveThread();
    }

    void StartReceiveThread()
    {
        receiveThread = new Thread(ReceiveLoop);

        receiveThread.Start();
    }

    void ReceiveLoop()
    {
        byte[] buffer =
            new byte[1024];

        while (true)
        {
            int size =
                stream.Read(
                    buffer,
                    0,
                    buffer.Length);

            string msg =
                Encoding.UTF8.GetString(
                    buffer,
                    0,
                    size);

            AddMessage(
                "Recebido: " + msg);

            HandleMessage(msg);
        }
    }

    void HandleMessage(string msg)
    {
        if (string.IsNullOrEmpty(msg)) return;

        string[] parts = msg.Split(';');
        if (parts.Length == 0) return;

        string cmd = parts[0];

        if (cmd == "MOVE" && parts.Length >= 6)
        {
            int fromX = int.Parse(parts[1]);
            int fromY = int.Parse(parts[2]);
            int toX = int.Parse(parts[3]);
            int toY = int.Parse(parts[4]);
            string cor = parts[5];

            MainThreadAction(() => ApplyRemoteMove(fromX, fromY, toX, toY, cor));
            MainThreadAction(() => Debug.Log($"Peça se mexeu"));
        }
        else if (cmd == "CAPTURE" && parts.Length >= 8)
        {
            int fromX = int.Parse(parts[1]);
            int fromY = int.Parse(parts[2]);
            int capturedX = int.Parse(parts[3]);
            int capturedY = int.Parse(parts[4]);
            int toX = int.Parse(parts[5]);
            int toY = int.Parse(parts[6]);
            string moverCor = parts[7];

            MainThreadAction(() => ApplyRemoteCapture(fromX, fromY, capturedX, capturedY, toX, toY, moverCor));
            MainThreadAction(() => Debug.Log($"Peça se mexeu"));
        }
    }

    Queue<System.Action> actions = new Queue<System.Action>();

    void MainThreadAction(System.Action action)
    {
        lock(actions)
        {
            actions.Enqueue(action);
        }
    }

    void Send(string msg)
    {
        if(stream == null) return;

        byte[] data = Encoding.UTF8.GetBytes(msg);

        stream.Write(data, 0, data.Length);

        AddMessage("Enviado: " + msg);
    }

    // Send serialized message over the stream
    void SendPosition(string msg)
    {
        if (stream == null) return;

        Send(msg);
    }

    void AddMessage(string msg)
    {
        lock(messages)
        {
            messages.Enqueue(msg);
        }
    }

    void Update()
    {
        lock (messages)
        {
            while (messages.Count > 0)
            {
                logText.text +=
                    "\n" +
                    messages.Dequeue();
            }
        }

        lock (actions)
        {
            while(actions.Count > 0)
            {
                actions.Dequeue().Invoke();
            }
        }
    }

    void OnDestroy()
    {
        receiveThread?.Abort();
        networkThread?.Abort();

        stream?.Close();
        client?.Close();
        listener?.Stop();
    }
    
    public void GetWhiteCheckers(Button checker)
    {
        pecasBrancas.Add(checker);
    }

    public void GetBlackCheckers(Button checker)
    {
        pecasPretas.Add(checker);
    }

    // ---- Helpers para aplicar movimentos recebidos na main thread ----
    void ApplyRemoteMove(int fromX, int fromY, int toX, int toY, string cor)
    {
        // Encontrar peça pela posição de origem na lista correta
        List<Button> lista = cor == "branco" ? pecasBrancas : pecasPretas;
        Button mover = null;
        foreach (Button b in lista)
        {
            if (b == null) continue;
            TilePosition tp = b.GetComponent<TilePosition>();
            if (tp != null && tp.x == fromX && tp.y == fromY)
            {
                mover = b;
                break;
            }
        }

        // fallback: procurar em cena (caso listas não contenham)
        if (mover == null)
        {
            Dama[] todas = FindObjectsOfType<Dama>();
            foreach (Dama d in todas)
            {
                TilePosition tp = d.GetComponent<TilePosition>();
                if (tp != null && tp.x == fromX && tp.y == fromY && d.time.ToString() == cor)
                {
                    mover = d.GetComponent<Button>();
                    break;
                }
            }
        }

        if (mover == null) return;

        // atualizar posição lógica
        TilePosition moverTP = mover.GetComponent<TilePosition>();
        moverTP.x = toX;
        moverTP.y = toY;

        // mover transform para tile correspondente
        GameObject tab = GameObject.Find("Tabuleiro");
        if (tab == null) return;
        Tabuleiro t = tab.GetComponent<Tabuleiro>();
        if (t == null) return;

        GameObject targetTile = t.espacos[toY, toX];
        Vector3 pos = targetTile.transform.position;
        pos.z = -1f;
        mover.transform.position = pos;

        // notificar mudança de turno localmente
        GameOM.MovePeca();
    }

    void ApplyRemoteCapture(int fromX, int fromY, int capturedX, int capturedY, int toX, int toY, string moverCor)
    {
        // mover peça atacante
        List<Button> lista = moverCor == "branco" ? pecasBrancas : pecasPretas;
        Button mover = null;
        foreach (Button b in lista)
        {
            if (b == null) continue;
            TilePosition tp = b.GetComponent<TilePosition>();
            if (tp != null && tp.x == fromX && tp.y == fromY)
            {
                mover = b;
                break;
            }
        }

        if (mover == null)
        {
            Dama[] todas = FindObjectsOfType<Dama>();
            foreach (Dama d in todas)
            {
                TilePosition tp = d.GetComponent<TilePosition>();
                if (tp != null && tp.x == fromX && tp.y == fromY && d.time.ToString() == moverCor)
                {
                    mover = d.GetComponent<Button>();
                    break;
                }
            }
        }

        GameObject tab = GameObject.Find("Tabuleiro");
        if (tab == null) return;
        Tabuleiro t = tab.GetComponent<Tabuleiro>();
        if (t == null) return;

        if (mover != null)
        {
            TilePosition moverTP = mover.GetComponent<TilePosition>();
            moverTP.x = toX;
            moverTP.y = toY;
            GameObject targetTile = t.espacos[toY, toX];
            Vector3 pos = targetTile.transform.position;
            pos.z = -1f;
            mover.transform.position = pos;
        }

        // destruir peça capturada (procurar nas listas)
        Button captured = null;
        List<Button> listaCapturados = moverCor == "branco" ? pecasPretas : pecasBrancas; // peça capturada é do time oposto
        foreach (Button b in listaCapturados)
        {
            if (b == null) continue;
            TilePosition tp = b.GetComponent<TilePosition>();
            if (tp != null && tp.x == capturedX && tp.y == capturedY)
            {
                captured = b;
                break;
            }
        }

        if (captured == null)
        {
            Dama[] todas = FindObjectsOfType<Dama>();
            foreach (Dama d in todas)
            {
                TilePosition tp = d.GetComponent<TilePosition>();
                if (tp != null && tp.x == capturedX && tp.y == capturedY)
                {
                    captured = d.GetComponent<Button>();
                    break;
                }
            }
        }

        if (captured != null)
        {
            // detectar cor da peça capturada para notificar GameManager
            Dama damaCapturada = captured.GetComponent<Dama>();
            enumCor corCapturada = enumCor.vazio;
            if (damaCapturada != null) corCapturada = damaCapturada.time;

            Destroy(captured.gameObject);
            GameOM.CheckerCollected(corCapturada);
        }

        // notificar mudança de turno localmente
        GameOM.MovePeca();
    }
}
