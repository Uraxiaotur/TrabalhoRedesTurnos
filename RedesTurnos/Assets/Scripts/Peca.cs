using UnityEngine;

public enum enumCor { vazio = 0, branco = 1, preto = 2}

public class Peca
{
    public int x {get; set;} //posição inicial da linha
    public int y {get; set;} //posição inicial da coluna
    public enumCor cor;
    private int trocaX; //posição da linha ao click
    private int trocaY; //posição da coluna ao click

    public Peca(int x, int y, enumCor cor)
    {
        this.x = x;
        this.y = y;
        this.cor = cor;
    }

}
