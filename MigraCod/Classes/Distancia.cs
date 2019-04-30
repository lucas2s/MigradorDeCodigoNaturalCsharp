/* Tabalho de Conclusão de Curso 
 * Pontífícia Universidade Católica de Minas Gerais
 * Curso de bacharelado em Sistemas de Informação
 * Prof: Julio 
 * 
 * Lucas Silva Sartori
 * 
 * Classe responsável calcular a distancia entre as variáveis e funções do código estruturado.
 * É chamada pela classe que realiza o agrupamento entre os termos e 
 * 
 * 
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MigraCod.Classes
{
    public class Distancia
    {
        private ArrayList grupo1;                    
        private ArrayList grupo2;
        private ArrayList rst_intercesao;
        private ArrayList rst_uniao;
        private double tot_intersecao;
        private double tot_uniao;
        private double rst_distancia;

        public Distancia()
        {
            grupo1 = new ArrayList();
            grupo2 = new ArrayList();
            rst_intercesao = new ArrayList();
            rst_uniao = new ArrayList();
            tot_intersecao = 0;
            tot_uniao = 0;
            rst_distancia = 0;
        }

        ~Distancia()  
        {
            //Destrutor
        }

        public ArrayList get_Set_Grupo1
        {
            get {return grupo1;}
            set {grupo1.AddRange(value);}
        }

        public ArrayList get_Set_Grupo2
        {
            get { return grupo2; }
            set { grupo2.AddRange(value); }
        }

        public double get_Tot_Intercesao
        {
            get { return tot_intersecao; }
        }

        public double get_Tot_Uniao
        {
            get { return tot_uniao; }
        }

        public double get_Rst_Distancia
        {
            get { return rst_distancia; }
        }

        private void calcula_Intercecao()
        {
            int index_gp2;
            int tam_grupo1 = this.grupo1.Count;
            int tam_grupo2 = this.grupo2.Count;
            for (int i = 0; i < tam_grupo1; i++)
            {
                index_gp2 = 0;
                index_gp2 = this.grupo2.IndexOf(this.grupo1[i]);
                if (index_gp2 >= 0)
                {
                     this.rst_intercesao.Add(this.grupo1[i]);
                }
            }
            this.tot_intersecao = this.rst_intercesao.Count;
        }

        private void calcula_Uniao()
        {
            {
                int index_gp1;
                int tam_grupo2 = this.grupo2.Count;
                this.rst_uniao.AddRange(grupo1);
                for (int i = 0; i < tam_grupo2; i++)
                {
                    index_gp1 = 0;
                    index_gp1= this.grupo1.IndexOf(this.grupo2[i]);
                    if (index_gp1 == -1)
                    {
                        this.rst_uniao.Add(this.grupo2[i]);
                    }
                }
            }
            this.tot_uniao = this.rst_uniao.Count;
        }

        public void caucula_Distancia()
        {
            calcula_Intercecao();
            calcula_Uniao();
            try
            {
                rst_distancia = 1 - (tot_intersecao / tot_uniao);
            }
            catch (Exception calc_erro)
            {
                throw (calc_erro);
            }
        }
    }
}
