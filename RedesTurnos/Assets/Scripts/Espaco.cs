using UnityEngine;

public class Espaco : Peca
{
    public Espaco(int x, int y, enumCor cor) : base(x, y, cor)
    {
        this.cor = enumCor.vazio;
    }
}
