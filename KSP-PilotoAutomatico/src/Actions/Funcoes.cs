using System;
using System.Text;

namespace KSP_PilotoAutomatico.Actions
{
    public static class Funcoes
    {
        public static int GetOpt()
        {
            StringBuilder mensagem = new StringBuilder();
            mensagem.Append("\n\nSelecione  ação: \n");
            mensagem.Append("0: Sair \n");
            mensagem.Append("1: Voo sub orbital \n");
            mensagem.Append("2: Voo orbital \n");
            mensagem.Append("3: Executar Manobra \n");

            Console.Write(mensagem);

            int opt = int.Parse(Console.ReadLine());

            return opt;
        }
    }

}
