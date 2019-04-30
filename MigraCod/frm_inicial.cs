using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using MigraCod.Classes;

namespace MigraCod
{
    public partial class frm_inicial : Form
    {
        public frm_inicial()
        {
            InitializeComponent();
        }

        private void bt_intermediario_Click(object sender, EventArgs e)
        {

            string ax_cod_entrada, ax_distancia;
            double vlr_distancia;

            rtb_intermediário.Text = "";
            rtb_para.Text = "";

            ax_cod_entrada = rtb_de.Text;
            ax_distancia = tb_distancia.Text;

            if (ax_distancia == "")
            {
                vlr_distancia = 0;
            }
            else
            {
                vlr_distancia = Double.Parse(ax_distancia);
            }

            Parse obj_parse = new Parse(vlr_distancia, ax_cod_entrada);
            Dictionary<string, ArrayList> conjuntos;
            Dictionary<string, ArrayList> variaveis_saida;
            Dictionary<int, List<string>> classes_saida;

            obj_parse.identificar_Variaveis_Funcoes();
            obj_parse.dividir_Codigo_Natural();
            obj_parse.formar_Conjuntos_Funcoes();
            obj_parse.formar_Conjuntos_Variaveis();
            obj_parse.calcular_Gerar_Agrupamentos();

            conjuntos = obj_parse.get_Conjuntos();
            classes_saida = obj_parse.get_Grupos_Saida();
            variaveis_saida = obj_parse.get_Variaveis();

            rtb_intermediário.Text += "***** CONJUNTOS VARIAVEIS E FUNCOES ****** \n";
            foreach (KeyValuePair<string, ArrayList> conj in conjuntos)
            {
                rtb_intermediário.Text += conj.Key + "={";
                for (int i = 0; i < conj.Value.Count; i++)
                {
                    rtb_intermediário.Text += conj.Value[i] + ", ";
                }
                rtb_intermediário.Text += "} \n";
                rtb_intermediário.Text += "------------------------ \n";
            }

            rtb_intermediário.Text += "***** VARIAVEIS ****** \n";
            foreach (KeyValuePair<string, ArrayList> vari_saida in variaveis_saida)
            {
                rtb_intermediário.Text += vari_saida.Key + " = ";
                for (int i = 0; i < vari_saida.Value.Count; i++)
                {
                    rtb_intermediário.Text += vari_saida.Value[i] + " ";
                }
                rtb_intermediário.Text += "\n";
                rtb_intermediário.Text += "------------------------ \n";
            }

            rtb_intermediário.Text += "\n\n";
            rtb_intermediário.Text += "***** CLASSES ****** \n";
            foreach (KeyValuePair<int, List<string>> conj in classes_saida)
            {
                rtb_intermediário.Text += "Classe "+ conj.Key + "={";
                for (int i = 0; i < conj.Value.Count; i++)
                {
                    rtb_intermediário.Text += conj.Value[i] + ", ";
                }
                rtb_intermediário.Text += "} \n";
                rtb_intermediário.Text += "------------------------ \n";
            }

            rtb_para.Text = obj_parse.get_cod_Csharp();

            foreach (KeyValuePair<string, string> conj in obj_parse.funcoes_codigo)
            {
                rtb_intermediário.Text += "Codigo"+ conj.Key+"\n";
                rtb_intermediário.Text += conj.Value +"\n ";
                rtb_intermediário.Text += "------------------------ \n";
            }

            rtb_intermediário.Text += "\n\n";
            rtb_intermediário.Text += "***** CODIGO MIGRADO ****** \n";

            foreach (KeyValuePair<string, ArrayList> FUNC in obj_parse.funcoes)
            {
                rtb_intermediário.Text += "FUNC " + FUNC.Key + "={";
                for (int i = 0; i < FUNC.Value.Count; i++)
                {
                    rtb_intermediário.Text += FUNC.Value[i] + ", ";
                }
                rtb_intermediário.Text += "} \n";
                rtb_intermediário.Text += "------------------------ \n";
            }
        }
    }
}
