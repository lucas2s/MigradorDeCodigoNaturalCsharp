using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MigraCod.Classes
{
    public class Agrupamento
    {
        private struct key_composta
        {
            private string key1;
            private string key2;

            public key_composta(string key1, string key2)
            {
                this.key1 = key1;
                this.key2 = key2;
            }

            public void alterar_Keys(string key1, string key2)
            {
                this.key1 = key1;
                this.key2 = key2;
            }

            public string get_Key1()
            {
                return this.key1;
            }

            public string get_key2()
            {
                return this.key2;
            }
        }

        private List <string> chaves_conjuntos;
        private Dictionary<string, ArrayList> conjuntos;
        private Dictionary<key_composta, double> distancia;
        private Dictionary<int, List<string>> grupos_saida;
        private double vlr_distancia;

        public Agrupamento(double vlr_distancia)
        {
            this.chaves_conjuntos = new List<string>();
            this.conjuntos = new Dictionary<string, ArrayList>();
            this.distancia = new Dictionary<key_composta, double>();
            this.grupos_saida = new Dictionary<int, List<string>>();

            if (vlr_distancia == 0)
            {
                this.vlr_distancia = 0.66666666666666;
            }
            else
            {
                this.vlr_distancia = vlr_distancia;
            }
        }

        public Dictionary<string, ArrayList> get_Conjuntos()
        {
            return this.conjuntos;
        }

        public Dictionary<int, List<string>> get_Grupos_Saida()
        {
            return this.grupos_saida;
        }

        public double get_distantica_conjuntos(string key1, string key2)
        {
            key_composta ax_chave_dist = new key_composta();

            ax_chave_dist.alterar_Keys(key1, key2);
            return this.distancia[ax_chave_dist];
        }

        public void incrementa_Conjunto(string key_conjunto, string valor_conjunto)
        {
            bool testa_key = this.conjuntos.ContainsKey(key_conjunto);
            int testa_valor;
            if (testa_key)
            {
                testa_valor = this.conjuntos[key_conjunto].IndexOf(valor_conjunto);
                if (testa_valor < 0)
                    this.conjuntos[key_conjunto].Add(valor_conjunto);
            }else
            {
                this.chaves_conjuntos.Add(key_conjunto);
                this.conjuntos.Add(key_conjunto, new ArrayList());
                this.conjuntos[key_conjunto].Add(valor_conjunto);
            }
        }

        private void distancia_Conjuntos(string key1, string key2)
        {
            Distancia calc_distancia = new Distancia();
            key_composta ax_chave_dist = new key_composta();

            ax_chave_dist.alterar_Keys(key1, key2);
            calc_distancia.get_Set_Grupo1 = this.conjuntos[key1.ToString()];
            calc_distancia.get_Set_Grupo2 = this.conjuntos[key2.ToString()];
            calc_distancia.caucula_Distancia();
            distancia.Add(ax_chave_dist, calc_distancia.get_Rst_Distancia);

        }

        public void calcular_Distancia_Conjuntos()
        {
            for (int i = 0; i < this.chaves_conjuntos.Count; i++)
                for (int y = 0; y < this.chaves_conjuntos.Count; y++)
                {
                    this.distancia_Conjuntos(this.chaves_conjuntos[i], this.chaves_conjuntos[y]);
                }
        }

        public void agrupamento_Gera_Classes()
        {
            int qtd_classes;
            string ax_elemento;
            qtd_classes = grupos_saida.Count();
            key_composta ax_chave_dist = new key_composta();

            if (qtd_classes == 0)
            {
                grupos_saida.Add(qtd_classes, chaves_conjuntos);
            }

            for (int z = 0; z < grupos_saida.Count(); z++)
            {
                for (int y = 1; y < grupos_saida[z].Count; y++)
                {
                    ax_chave_dist.alterar_Keys(grupos_saida[z][0], grupos_saida[z][y]);
                    if (distancia[ax_chave_dist] > this.vlr_distancia)
                    {
                        ax_elemento = grupos_saida[z][y];
                        grupos_saida[z].Remove(grupos_saida[z][y]);
                        y--;
                        qtd_classes = z + 1;
                        if (qtd_classes == grupos_saida.Count())
                        {
                            grupos_saida.Add(qtd_classes, new List<string>());
                            grupos_saida[qtd_classes].Add(ax_elemento);
                        }
                        else if (qtd_classes < grupos_saida.Count())
                        {
                            grupos_saida[qtd_classes].Add(ax_elemento);
                        }
                    }
                }
            }
        }

        public void trata_Elemento_Unico()
        {
            string ax_elemente;
            double ax_menor = 1;
            int ax_grupo = 0;
            key_composta ax_chave_dist = new key_composta();

            for (int i = 0; i < grupos_saida.Count; i++)
            {
                if (grupos_saida[i].Count == 1)
                {
                    ax_elemente = grupos_saida[i][0];
                    grupos_saida[i].Remove(ax_elemente);
                    for (int y = 0; y < grupos_saida.Count; y++)
                    {
                        for (int z = 0; z < grupos_saida[y].Count; z++)
                        {
                            ax_chave_dist.alterar_Keys(ax_elemente, grupos_saida[y][z]);
                            if (distancia[ax_chave_dist] < ax_menor)
                            {
                                ax_menor = distancia[ax_chave_dist];
                                ax_grupo = y;
                            }
                        }
                    }
                    grupos_saida[ax_grupo].Add(ax_elemente);
                }
            }
            for (int i = 0; i < grupos_saida.Count; i++)
            {
                if (grupos_saida[i].Count == 0)
                {
                    grupos_saida.Remove(i);
                }
            }
        }
    }
}
