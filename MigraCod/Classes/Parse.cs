/* Tabalho de Conclusão de Curso 
 * Pontífícia Universidade Católica de Minas Gerais
 * Curso de bacharelado em Sistemas de Informação
 * Prof: Julio 
 * 
 * Lucas Silva Sartori
 * 
 * Classe responsável pela migração do Código Natural para C#
 * Utiliza a classe Agrupamento para caucular os grupos da classes de saída do código estruturado.
 * 
 * 
 * 
 */




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace MigraCod.Classes
{
    class Parse:Agrupamento
    {
        private String entrada_natural;
        private String saida_csharp;
        private Dictionary<string, string> nat_reservadas;
        private Dictionary<string, ArrayList> variaveis;
        private Dictionary<string, List<int>> variaveis_vet;
        private Dictionary<int, List<string>> classes_saida;
        public  Dictionary<string, ArrayList> funcoes;
        public  Dictionary<string, string> funcoes_codigo;
        private string identacao;

        public Parse(double vlr_distancia, String tx_entrada): base(vlr_distancia)
        {
           
            nat_reservadas = new Dictionary<string, string>();
            variaveis = new Dictionary<string, ArrayList>();
            funcoes = new Dictionary<string, ArrayList>();
            funcoes_codigo = new Dictionary<string, string>();
            variaveis_vet = new Dictionary<string, List<int>>();

            // COMANDOS RESERVADOS DO NATURAL E OS RESPECTIVOS COMANDOS NO C#
            nat_reservadas.Add("DEFINE", "");
            nat_reservadas.Add("DATA", "");
            nat_reservadas.Add("LOCAL", "");
            nat_reservadas.Add("END-DEFINE", "");
            nat_reservadas.Add("IF", "");
            nat_reservadas.Add("END-IF", "}");
            nat_reservadas.Add("ELSE", "else");
            nat_reservadas.Add("REPEAT", "");
            nat_reservadas.Add("END-REPEAT", "}");
            nat_reservadas.Add("WHILE", "");
            nat_reservadas.Add("DECIDE", "");
            nat_reservadas.Add("END-DECIDE", "}");
            nat_reservadas.Add("VALUE", "");
            nat_reservadas.Add("NONE", "");
            nat_reservadas.Add("SUBROUTINE", "");
            nat_reservadas.Add("END-SUBROUTINE", "}");
            nat_reservadas.Add("REDEFINE", "");
            nat_reservadas.Add("PERFORM", "");
            nat_reservadas.Add("FOR", "");
            nat_reservadas.Add("END-FOR", "}");
            nat_reservadas.Add("ADD", "");
            nat_reservadas.Add("COMPUTE", "");            
            nat_reservadas.Add("TRUE", "true");
            nat_reservadas.Add("FALSE", "false");
            nat_reservadas.Add("WRITE", "");
            nat_reservadas.Add("TO", "");
            nat_reservadas.Add("INTO", "");
            nat_reservadas.Add("MOVE", "");
            nat_reservadas.Add("INPUT", "");
            nat_reservadas.Add("'", "\"");


            // OPERADORES RESERVADOS DO NATURAL E OS RESPECTIVOS OPERADORES NO C#
            nat_reservadas.Add("EQ", "==");
            nat_reservadas.Add("NE", "!=");
            nat_reservadas.Add("GT", ">");
            nat_reservadas.Add("LT", "<");
            nat_reservadas.Add("GE", ">=");
            nat_reservadas.Add("LE", "<=");
            nat_reservadas.Add("OR", "||");
            nat_reservadas.Add("AND", "&&");
            nat_reservadas.Add("=", "==");

            // REMOVER ESPAÇOS DESNECESSÁRIOS DO CÓDIGO NATURAL
            this.entrada_natural = formatar_Codigo(tx_entrada);

            // INICIALIZAR AS FUNCOES
            funcoes.Add("PRINCIPAL", new ArrayList());
            funcoes["PRINCIPAL"].Add("public");
            funcoes["PRINCIPAL"].Add("void");
            funcoes["PRINCIPAL"].Add("Func_Principal");
            funcoes["PRINCIPAL"].Add("()");
            funcoes["PRINCIPAL"].Add("{");
        }

        private string formatar_Codigo(string tx_entrada)
        {
            tx_entrada = tx_entrada.Replace("\n", " ");
            tx_entrada = tx_entrada.Replace("(", " ( ");
            tx_entrada = tx_entrada.Replace(")", " ) ");
            tx_entrada = tx_entrada.Replace(":=", " := ");
            tx_entrada = tx_entrada.Replace("+", " + ");
            tx_entrada = tx_entrada.Replace("*", " * ");
            tx_entrada = tx_entrada.Replace("/", " / ");
            tx_entrada = tx_entrada.Replace("'", " ' ");
            tx_entrada = tx_entrada.Replace(",", " , ");
            
            while (tx_entrada.IndexOf("  ") != -1)
            {
                tx_entrada = tx_entrada.Replace("  ", " ");
            }

            tx_entrada = tx_entrada.ToUpper();

            return tx_entrada;
        }

        public Dictionary<string, ArrayList> get_Variaveis()
        {
            return this.variaveis;
        }

        public string get_cod_Natural()
        {
            return this.entrada_natural;
        }

        public string get_cod_Csharp()
        {
            return this.saida_csharp;
        }

        public void identificar_Variaveis_Funcoes()
        {
            string[] array_palavras;
            array_palavras = this.entrada_natural.Split();
            for (int i = 0; i < array_palavras.Length; i++)
            {

                switch (array_palavras[i])
                {
                    case "DEFINE":
                        if(array_palavras[i+1] == "SUBROUTINE")
                        {
                            funcoes.Add(array_palavras[i + 2], new ArrayList());
                            funcoes[array_palavras[i + 2]].Add("public");
                            funcoes[array_palavras[i + 2]].Add("void");
                            funcoes[array_palavras[i + 2]].Add("Func_" + array_palavras[i + 2].ToLower());
                            funcoes[array_palavras[i + 2]].Add("()");
                            funcoes[array_palavras[i + 2]].Add("{");
                        }
                        if(array_palavras[i+1] == "DATA" && array_palavras[i+2] == "LOCAL")
                        {
                            identificar_Variaveis(array_palavras, ref i);
                        }
                    break;

                }
            }
        }

        public void identificar_Variaveis(string[] array_palavras, ref int posicao)
        {
            char [] tipo_var;
            bool ind_nat_reservadas, ind_numerico, ind_variavel, ind_numerico_ant, tem_ponto;
            string var_aux, tipo_aux="";
            int ind_vet = 0;
            posicao = 1;
            while (array_palavras[posicao] != "END-DEFINE")
            {
                ind_nat_reservadas = nat_reservadas.ContainsKey(array_palavras[posicao]);
                ind_numerico = valida_Numero(array_palavras[posicao]);
                ind_numerico_ant = valida_Numero(array_palavras[posicao - 1]);
                ind_variavel = valida_Variavel(array_palavras[posicao]);
                if (!(ind_nat_reservadas && ind_numerico) && (ind_variavel && ind_numerico_ant))
                {
                    variaveis.Add(array_palavras[posicao], new ArrayList());
                    variaveis[array_palavras[posicao]].Add("private");
                    tipo_var = array_palavras[posicao + 2].ToCharArray();
                    tem_ponto = array_palavras[posicao + 2].Contains(".");
                    if (tem_ponto && tipo_var[0].ToString() == "N")
                    {
                        tipo_aux = "double";
                        variaveis[array_palavras[posicao]].Add(tipo_aux);
                    }
                    else
                    {
                        switch (tipo_var[0].ToString())
                        {
                            case "N":
                                tipo_aux = "int";
                                variaveis[array_palavras[posicao]].Add(tipo_aux);
                                break;
                            case "A":
                                tipo_aux = "string";
                                variaveis[array_palavras[posicao]].Add(tipo_aux);
                                break;
                            case "L":
                                tipo_aux = "bool";
                                variaveis[array_palavras[posicao]].Add(tipo_aux);
                                break;
                            case "P":
                                tipo_aux = "int";
                                variaveis[array_palavras[posicao]].Add(tipo_aux);
                                break;
                            case "I":
                                tipo_aux = "int";
                                variaveis[array_palavras[posicao]].Add(tipo_aux);
                                break;
                            case "F":
                                tipo_aux = "double";
                                variaveis[array_palavras[posicao]].Add(tipo_aux);
                                break;
                        }
                    }
                    var_aux = array_palavras[posicao].ToLower();
                    var_aux = var_aux.Replace("#", "var_");
                    if (array_palavras[posicao + 3] == "/") 
                    {
                        variaveis_vet.Add(array_palavras[posicao], new List<int>());
                        variaveis_vet[array_palavras[posicao]].Add(4);
                        variaveis_vet[array_palavras[posicao]].Add(4);
                        ind_vet = posicao + 4;
                        variaveis[array_palavras[posicao]].Add("[");
                        while (array_palavras[ind_vet] != ")")
                        {
                            if (array_palavras[ind_vet] == ",")
                            {
                                variaveis[array_palavras[posicao]].Add(",");
                                variaveis_vet[array_palavras[posicao]][0]++;
                                variaveis_vet[array_palavras[posicao]][1] += 2;
                            }
                            ind_vet++;
                        }
                        variaveis[array_palavras[posicao]].Add("]");
                        variaveis[array_palavras[posicao]].Add(var_aux.ToLower());
                        variaveis[array_palavras[posicao]].Add("= new");
                        variaveis[array_palavras[posicao]].Add(tipo_aux);
                        ind_vet = posicao + 4;
                        variaveis[array_palavras[posicao]].Add("[");
                        while (array_palavras[ind_vet] != ")")
                        {
                            if (array_palavras[ind_vet] == ",")
                            {
                                variaveis[array_palavras[posicao]].Add(",");
                            }
                            else
                            {
                                variaveis[array_palavras[posicao]].Add(array_palavras[ind_vet]);
                            }
                            ind_vet++;
                        }
                        variaveis[array_palavras[posicao]].Add("]");
                    }
                    else
                    {
                        variaveis[array_palavras[posicao]].Add(var_aux.ToLower());
                    }
                    variaveis[array_palavras[posicao]].Add(";");
                    posicao++;
                }
                posicao++;
            }
        }

        public void dividir_Codigo_Natural()
        {
            string ax_cod_nat = this.entrada_natural;
            string ax_cod_div;

            int ind_inicio = 0, ind_fim = 0, ind_tamanho = 0; 

            foreach( KeyValuePair<string, ArrayList> func in this.funcoes)
            {
                if (func.Key == "PRINCIPAL")
                {
                    ind_inicio = ax_cod_nat.IndexOf("END-DEFINE");
                    ind_fim = ax_cod_nat.IndexOf("DEFINE SUBROUTINE", ind_inicio + 1);
                    if (ind_fim == -1)
                        ind_fim = ax_cod_nat.Length;

                    ind_tamanho = ind_fim - ind_inicio;

                    ax_cod_div = ax_cod_nat.Substring(ind_inicio, ind_tamanho);
                    funcoes_codigo.Add(func.Key, ax_cod_div);
                }
                else
                {
                    ind_inicio = ax_cod_nat.IndexOf(func.Key, ind_inicio + ind_tamanho);
                    ind_fim = ax_cod_nat.IndexOf("END-SUBROUTINE", ind_inicio + 1);
                    ind_tamanho = ind_fim - ind_inicio;
                    ax_cod_div = ax_cod_nat.Substring(ind_inicio, ind_tamanho);
                    funcoes_codigo.Add(func.Key, ax_cod_div);
                } 
            }
         }


        public void formar_Conjuntos_Funcoes()
        {
            string[] array_palavras;
            bool ind_variavel, ind_funcao;
            int existe_funcao;

            foreach (KeyValuePair<string, ArrayList> funcao in this.funcoes)
            {
                this.incrementa_Conjunto(funcao.Key, funcao.Key);

                foreach (KeyValuePair<string, string> func_cod in this.funcoes_codigo)
                {
                    array_palavras = func_cod.Value.Split();
                    existe_funcao = Array.IndexOf(array_palavras, funcao.Key);
                    if(existe_funcao >= 0)
                    {
                        this.incrementa_Conjunto(funcao.Key, func_cod.Key);
                        for (int i = 0; i < array_palavras.Length; i++)
                        {
                            ind_variavel = variaveis.ContainsKey(array_palavras[i]);
                            ind_funcao = this.funcoes.ContainsKey(array_palavras[i]);
                            if ((ind_funcao || ind_variavel) && (array_palavras[i] != funcao.Key))
                            {
                                this.incrementa_Conjunto(func_cod.Key, array_palavras[i]);
                            }
                            else
                            {
                                if (array_palavras[i] == "PERFORM")
                                {
                                    ind_funcao = this.funcoes.ContainsKey(array_palavras[i + 1]);
                                    if (ind_funcao)
                                    {
                                        this.incrementa_Conjunto(func_cod.Key, array_palavras[i + 1]);
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void formar_Conjuntos_Variaveis()
        {
            string[] array_palavras;
            bool ind_funcao, ind_variavel;
            int existe_variavel;

            foreach (KeyValuePair<string, ArrayList> variavel in this.variaveis)
            {
                this.incrementa_Conjunto(variavel.Key, variavel.Key);
                foreach (KeyValuePair<string, string> func_cod in this.funcoes_codigo)
                {
                    array_palavras = func_cod.Value.Split();
                    existe_variavel = Array.IndexOf(array_palavras, variavel.Key);
                    if (existe_variavel >= 0)
                    {
                        this.incrementa_Conjunto(variavel.Key, func_cod.Key);
                        for (int i = 0; i < array_palavras.Length; i++)
                        {
                            ind_variavel = variaveis.ContainsKey(array_palavras[i]);
                            ind_funcao = funcoes.ContainsKey(array_palavras[i]);
                            if ((ind_funcao || ind_variavel) && (array_palavras[i] != variavel.Key))
                            {
                                this.incrementa_Conjunto(variavel.Key, array_palavras[i]);
                            }
                        }
                    }
                }
            }
        }



        private bool valida_Numero(string numero)
        {
            if (numero != "")
            {
                Regex rx = new Regex(@"^[\d]*$|^[\d]\.[\d]*$");
                return rx.IsMatch(numero);
            }
            else
            {
                return false;
            }
        }

        private bool valida_Variavel(string variavel)
        {
            Regex rx = new Regex(@"^[#|A-Z][A-Z0-9]");
            return rx.IsMatch(variavel);
        }

        private bool valida_String(string variavel)
        {
            Regex rx = new Regex(@"^[|A-Z][A-Z0-9]");
            return rx.IsMatch(variavel);
        }

        public void calcular_Gerar_Agrupamentos()
        {
            this.calcular_Distancia_Conjuntos();
            this.agrupamento_Gera_Classes();
            this.trata_Elemento_Unico();
            this.classes_saida = this.get_Grupos_Saida();
            this.criar_Estrutura_Codigo();
            this.imprimir_Variaveis();
            this.imprimir_Get_Set_Variaaveis();
            this.migrar_Codigo_Funcoes();
            this.imprimir_Funcoes();
        }

        private void remover_Identacao()
        {
            int tam_ident;
            string ax_identacao;
            ax_identacao = this.identacao;
            this.identacao = "";
            tam_ident = ax_identacao.Length;
            this.identacao = ax_identacao.Substring(0, tam_ident - 1);
        }

        private void adicionar_Identacao()
        {
            this.identacao += "\t";
        }

        public void criar_Estrutura_Codigo()
        {
            this.saida_csharp += "namespace Migracao \n{";
            this.adicionar_Identacao();
            int ax_cont = 0;
            bool eh_principal;

            foreach (KeyValuePair<int, List<string>> classe in this.classes_saida)
            {
                eh_principal = this.classes_saida[classe.Key].Contains("PRINCIPAL");
                this.saida_csharp += "\n" + this.identacao + "class Classe_" + classe.Key;
                this.saida_csharp += "\n" + this.identacao + "{";
                if (eh_principal)
                {
                    this.adicionar_Identacao();
                    this.saida_csharp += "\n" + this.identacao + "static void Main (string[] args)";
                    this.saida_csharp += "\n" + this.identacao + "{";
                    this.adicionar_Identacao();
                    this.saida_csharp += "\n" + this.identacao + "Classe_" + classe.Key + " obj_classe_" + classe.Key + " = new Classe_" + classe.Key + "();";
                    this.saida_csharp += "\n" + this.identacao + "obj_classe_" + classe.Key + ".Func_Principal();";
                    this.saida_csharp += "\n" + this.identacao + "Console.ReadKey();";
                    this.remover_Identacao();
                    this.saida_csharp += "\n" + this.identacao + "}";
                    this.remover_Identacao();
                    ax_cont++;
                }
                this.saida_csharp += "\n" + " // Final_da_classe_"+ classe.Key;
                this.saida_csharp += "\n" + this.identacao + "}\n";
            }
            this.saida_csharp += "\n}";
            this.adicionar_Identacao();
        }

        public void migrar_Codigo_Funcoes()
        {
            string[] codigo_nat;
            bool eh_variavel, eh_vetor;
            foreach (KeyValuePair<string, ArrayList> func_codigo in this.funcoes)
            {
                codigo_nat = this.funcoes_codigo[func_codigo.Key].Split();

                for (int posicao = 0; posicao < codigo_nat.Length; )
                {
                    eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                    eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]); 
                    if (codigo_nat[posicao] != "")
                    {
                        if (eh_variavel)
                        {
                            if (codigo_nat[posicao + 1] == ":=")
                            {
                                migra_MATH(ref posicao, codigo_nat, func_codigo.Value);
                            }
                            else
                            {
                                if (eh_vetor && codigo_nat[posicao + variaveis_vet[codigo_nat[posicao]][1]] == ":=")
                                {
                                    migra_MATH(ref posicao, codigo_nat, func_codigo.Value);
                                }
                                else
                                {
                                    posicao++;
                                }
                            }
                        }
                        else
                        {
                            switch (codigo_nat[posicao])
                            {
                                case "IF":
                                    migra_IF(ref posicao, codigo_nat, func_codigo.Value);
                                    break;
                                case "MOVE":
                                    migra_MOVE(ref posicao, codigo_nat, func_codigo.Value);
                                    break;
                                case "PERFORM":
                                    migra_PERFORM(ref posicao, codigo_nat, func_codigo.Value);
                                    break;
                                case "WRITE":
                                    migra_WRITE(ref posicao, codigo_nat, func_codigo.Value);
                                    break;
                                case "FOR":
                                    migra_FOR(ref posicao, codigo_nat, func_codigo.Value);
                                    break;
                                case "REPEAT":
                                    migra_REPEAT(ref posicao, codigo_nat, func_codigo.Value);
                                    break;
                                case "INPUT":
                                    migra_INPUT(ref posicao, codigo_nat, func_codigo.Value);
                                    break;
                                case "DECIDE":
                                    migra_DECIDE(ref posicao, codigo_nat, func_codigo.Value);
                                    break;
                                default:
                                    posicao++;
                                    break;
                            }
                        }         
                    }
                    else
                    {
                        posicao++;
                    }
                }
                func_codigo.Value.Add("}");
            }
        }

        public void migra_VETOR(ref int posicao, string[] codigo_nat, ArrayList cod_funcao)
        {
            bool eh_numero, eh_variavel;
            int ax_ind = 0;

            cod_funcao.Add(this.variaveis[codigo_nat[posicao]][variaveis_vet[codigo_nat[posicao]][0]]);
            cod_funcao.Add("[");
            posicao += 2;
            while (codigo_nat[posicao] != ")")
            {
                eh_numero = valida_Numero(codigo_nat[posicao]);
                if (eh_numero)
                {
                    ax_ind = Convert.ToInt32(codigo_nat[posicao]) - 1;
                    cod_funcao.Add(Convert.ToString(ax_ind));
                }
                else
                {
                    eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                    if (eh_variavel)
                    {
                        cod_funcao.Add(this.variaveis[codigo_nat[posicao]][2]);
                    }
                    else
                    {
                        cod_funcao.Add(codigo_nat[posicao]);
                    }
                }
                posicao++;
            }
            cod_funcao.Add("]");
        }

        public void migra_VETOR(ref int posicao, string[] codigo_nat, ref string conteudo)
        {
            bool eh_numero, eh_variavel;
            int ax_ind = 0;

            conteudo += this.variaveis[codigo_nat[posicao]][variaveis_vet[codigo_nat[posicao]][0]];
            conteudo += " [" ;
            posicao += 2;
            while (codigo_nat[posicao] != ")")
            {
                eh_numero = valida_Numero(codigo_nat[posicao]);
                if (eh_numero)
                {
                    ax_ind = Convert.ToInt32(codigo_nat[posicao]) - 1;
                    conteudo +=  Convert.ToString(ax_ind);
                }
                else
                {
                    eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                    if (eh_variavel)
                    {
                        conteudo += this.variaveis[codigo_nat[posicao]][2];
                    }
                    else
                    {
                        conteudo += codigo_nat[posicao];
                    }
                }
                posicao++;
            }
            conteudo += " ]";
        }

        public void migra_MOVE(ref int posicao, string[] codigo_nat, ArrayList cod_funcao)
        {
            int ind_conteudo;
            bool eh_variavel, eh_vetor, eh_nat_reservadas, testa_final = false;
            string string_conteudo = "";

            // ENCONTRAR CONTEUDO
            ind_conteudo = posicao;
            ind_conteudo++;
            eh_variavel = this.variaveis.ContainsKey(codigo_nat[ind_conteudo]);
            eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[ind_conteudo]);
            eh_nat_reservadas = this.nat_reservadas.ContainsKey(codigo_nat[ind_conteudo]);

            if (eh_variavel)
            {
                if (eh_vetor)
                {
                    migra_VETOR(ref ind_conteudo, codigo_nat, ref string_conteudo);
                    posicao = ind_conteudo;
                }
                else
                {
                    string_conteudo = this.variaveis[codigo_nat[ind_conteudo]][2].ToString();
                }
            }
            else
            {
                if (codigo_nat[ind_conteudo] == "'")
                {
                    string_conteudo = "";
                    string_conteudo += "\"";
                    ind_conteudo++;
                    while (codigo_nat[ind_conteudo] != "'")
                    {
                        string_conteudo += codigo_nat[ind_conteudo];
                        ind_conteudo++;
                    }
                    string_conteudo += "\"";
                }
                else
                {
                    if (eh_nat_reservadas)
                    {
                        string_conteudo = this.nat_reservadas[codigo_nat[ind_conteudo]];
                    }
                    else
                    {
                        string_conteudo = codigo_nat[ind_conteudo];
                    }
                }
            }

            // ENCONTRAR A VARIAVEL QUE RECEBERA O VALOR DO MOVE
            while (codigo_nat[posicao] != "TO")
            {
                posicao++;
            }

            while (!testa_final)
            {
                posicao++;
                eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]);
                eh_nat_reservadas = this.nat_reservadas.ContainsKey(codigo_nat[posicao]);

                if (eh_variavel && !eh_nat_reservadas && !eh_vetor && codigo_nat[posicao + 1] != ":=")
                {
                    cod_funcao.Add(this.variaveis[codigo_nat[posicao]][2]);
                    cod_funcao.Add("=");
                    cod_funcao.Add(string_conteudo);
                    cod_funcao.Add(";");
                }
                else
                {
                    if (eh_vetor && codigo_nat[posicao + variaveis_vet[codigo_nat[posicao]][1]] != ":=")
                    {
                        migra_VETOR(ref posicao, codigo_nat, cod_funcao);
                        cod_funcao.Add("=");
                        cod_funcao.Add(string_conteudo);
                        cod_funcao.Add(";");
                    }
                    else
                    {
                        testa_final = true;
                    }
                }
            }
        }

        public void migra_WRITE(ref int posicao, string[] codigo_nat, ArrayList cod_funcao)
        {
            bool eh_variavel, eh_vetor, eh_nat_reservadas, testa_final = false;
            int ind_var, cont_variaveis = 0;
            string aux_variavel = "", ax_msg = "\"";
            ArrayList aux_variaveis = new ArrayList();
            cod_funcao.Add("Console.WriteLine");
            cod_funcao.Add("(");

            while (!testa_final)
            {
                posicao++;
                eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]);
                eh_nat_reservadas = this.nat_reservadas.ContainsKey(codigo_nat[posicao]);

                if (eh_variavel && !eh_nat_reservadas && !eh_vetor && codigo_nat[posicao + 1] != ":=")
                {
                    ax_msg += "{" + cont_variaveis + "} ";
                    cont_variaveis++;
                    aux_variaveis.Add(this.variaveis[codigo_nat[posicao]][2]);
                }
                else
                {
                    if (eh_vetor && codigo_nat[posicao + variaveis_vet[codigo_nat[posicao]][1]] != ":=")
                    {
                        ax_msg += "{" + cont_variaveis + "} "; ;
                        cont_variaveis++;
                        aux_variavel = "";
                        migra_VETOR(ref posicao, codigo_nat, ref aux_variavel);
                        aux_variaveis.Add(aux_variavel);
                    }
                    else
                    {
                        if (codigo_nat[posicao] == "'" && codigo_nat[posicao + 1] != "=")
                        {
                            posicao++;
                            while (codigo_nat[posicao] != "'")
                            {
                                ax_msg += codigo_nat[posicao] + " ";
                                posicao++;
                            }
                        }
                        else
                        {
                            if (codigo_nat[posicao] == "/")
                            {
                                ax_msg += "\\n";
                            }
                            else
                            {
                                if (codigo_nat[posicao] == "'" && codigo_nat[posicao + 1] == "=" && codigo_nat[posicao + 2] == "'")
                                {
                                    ind_var = posicao + 3;
                                    eh_variavel = this.variaveis.ContainsKey(codigo_nat[ind_var]);
                                    eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[ind_var]);
                                    if (eh_variavel)
                                    {
                                        ax_msg = ax_msg + this.variaveis[codigo_nat[ind_var]][2] + " = {" + cont_variaveis + "}";
                                        cont_variaveis++;
                                        aux_variaveis.Add(this.variaveis[codigo_nat[ind_var]][2]);
                                    }
                                    else
                                    {
                                        migra_VETOR(ref ind_var, codigo_nat, ref aux_variavel);
                                        ax_msg = ax_msg + "\"" + aux_variavel + "\" = {" + cont_variaveis + "}";
                                        cont_variaveis++;
                                        aux_variaveis.Add(aux_variavel);
                                    }
                                    posicao += 3;
                                }
                                else
                                {
                                    testa_final = true;
                                }
                            }
                        }
                    }
                }
            }

            if (ax_msg != "\"")
            {
                ax_msg += "\"";
                cod_funcao.Add(ax_msg);
            }

            if (aux_variaveis.Count > 0)
            {
                cod_funcao.Add(",");
                for (int i = 0; i < aux_variaveis.Count; i++)
                {
                    if (i > 0)
                    {
                        cod_funcao.Add(",");
                    }
                    cod_funcao.Add(aux_variaveis[i]);
                }
             }
            cod_funcao.Add(")");
            cod_funcao.Add(";");
        }

        public void migra_INPUT(ref int posicao, string[] codigo_nat, ArrayList cod_funcao)
        {
            bool eh_variavel, eh_vetor, eh_nat_reservadas, testa_final = false, ax_add_var = false;
            int ax_posicao;
            posicao++;

            while (!testa_final)
            {

                while (codigo_nat[posicao] == "/")
                {
                    posicao++;
                }
              
                eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]);
                eh_nat_reservadas = this.nat_reservadas.ContainsKey(codigo_nat[posicao]);
                ax_posicao = posicao;

                if (eh_vetor && codigo_nat[posicao + variaveis_vet[codigo_nat[posicao]][1]] != ":=")
                {
                    migra_VETOR(ref posicao, codigo_nat, cod_funcao);
                    cod_funcao.Add("=");
                    ax_add_var = true;
                }
                else
                {
                    if (eh_variavel && codigo_nat[posicao + 1] != ":=" && !eh_nat_reservadas && !eh_vetor)
                    {
                        cod_funcao.Add(this.variaveis[codigo_nat[posicao]][2]);
                        cod_funcao.Add("=");
                        ax_add_var = true;
                    }
                    else
                    {
                        if (codigo_nat[posicao] == "'" && codigo_nat[posicao + 1] != "=")
                        {
                            cod_funcao.Add("Console.WriteLine");
                            cod_funcao.Add("(");
                            cod_funcao.Add("\"");
                            posicao++;
                            while (codigo_nat[posicao] != "'")
                            {
                                cod_funcao.Add(codigo_nat[posicao]);
                                posicao++;
                            }
                            posicao++;
                            cod_funcao.Add("\"");
                            cod_funcao.Add(")");
                            cod_funcao.Add(";");
                        }
                        else
                        {
                            testa_final = true;
                        }
                    }
                }
                if (eh_variavel & ax_add_var)
                {
                    ax_add_var = false;
                    switch (this.variaveis[codigo_nat[ax_posicao]][1].ToString())
                    {
                        case "int":
                            cod_funcao.Add("Convert.ToInt32");
                            cod_funcao.Add("(");
                            cod_funcao.Add("Console.ReadLine");
                            cod_funcao.Add("(");
                            cod_funcao.Add(")");
                            cod_funcao.Add(")");
                            cod_funcao.Add(";");
                            break;
                        case "double":
                            cod_funcao.Add("Convert.ToDouble");
                            cod_funcao.Add("(");
                            cod_funcao.Add("Console.ReadLine");
                            cod_funcao.Add("(");
                            cod_funcao.Add(")");
                            cod_funcao.Add(")");
                            cod_funcao.Add(";");
                            break;
                        case "string":
                            cod_funcao.Add("Console.ReadLine");
                            cod_funcao.Add("(");
                            cod_funcao.Add(")");
                            cod_funcao.Add(";");
                            break;
                    }
                    posicao++;
                }
            }
        }


        public void migra_MATH(ref int posicao, string[] codigo_nat, ArrayList cod_funcao)
        {
            bool eh_variavel, eh_numero, eh_vetor;
            bool testa_final = false;

            eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]);

            if (eh_vetor)
            {
                migra_VETOR(ref posicao, codigo_nat, cod_funcao);    
            }
            else
            {
                cod_funcao.Add(this.variaveis[codigo_nat[posicao]][2]);
            }
            posicao++;

            while (testa_final != true)
            {
                eh_numero = valida_Numero(codigo_nat[posicao]);
                if ((codigo_nat[posicao] == "+" || codigo_nat[posicao] == "-" || codigo_nat[posicao] == ":=") ||
                    codigo_nat[posicao] == "*" || codigo_nat[posicao] == "/" || (eh_numero))
                {
                    if (codigo_nat[posicao] == ":=")
                    {
                        cod_funcao.Add("=");
                        posicao++;
                    }
                    else
                    {
                        cod_funcao.Add(codigo_nat[posicao]);
                        posicao++;
                    }
                }
                else
                {
                    eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                    eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]);
                    if (eh_variavel && !eh_vetor && codigo_nat[posicao + 1] != ":=")
                    {
                        cod_funcao.Add(this.variaveis[codigo_nat[posicao]][2]);
                        posicao++;
                    }
                    else 
                    {
                        if (eh_vetor && codigo_nat[posicao + variaveis_vet[codigo_nat[posicao]][1]] != ":=")
                        {
                            migra_VETOR(ref posicao, codigo_nat, cod_funcao);
                            posicao++;
                        }
                        else
                        {
                            cod_funcao.Add(";");
                            testa_final = true;
                        }
                    }
                }
            }
        }

        public void migra_PERFORM(ref int posicao, string[] codigo_nat, ArrayList cod_funcao)
        {
            posicao++;
            cod_funcao.Add(this.funcoes[codigo_nat[posicao]][2]);
            cod_funcao.Add("()");
            cod_funcao.Add(";");
            posicao++;
        }

        public void migra_IF(ref int posicao, string[] codigo_nat, ArrayList cod_funcao)
        {
            bool eh_variavel, eh_vetor, eh_nat_reservadas, eh_funcao, eh_numero;
            int cont = 0;

            bool testa_final = false;
            cod_funcao.Add("if");
            cod_funcao.Add("(");
            posicao++;

            while (testa_final != true)
            {
                eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]);
                eh_nat_reservadas = this.nat_reservadas.ContainsKey(codigo_nat[posicao]);
                eh_funcao = this.funcoes.ContainsKey(codigo_nat[posicao]);
                eh_numero = this.valida_Numero(codigo_nat[posicao]);

                if (eh_variavel)
                {
                    if (eh_vetor)
                    {
                        if (codigo_nat[posicao + variaveis_vet[codigo_nat[posicao]][1]] != ":=")
                        {
                            migra_VETOR(ref posicao, codigo_nat, cod_funcao);
                            posicao++;
                        }
                        else
                        {
                            migra_MATH(ref posicao, codigo_nat, cod_funcao);
                        }
                    }
                    else
                    {
                        if (codigo_nat[posicao + 1] != ":=")
                        {
                            cod_funcao.Add(this.variaveis[codigo_nat[posicao]][2]);
                            posicao++;
                        }
                        else
                        {
                            migra_MATH(ref posicao, codigo_nat, cod_funcao);
                        }
                    }
                }
                else
                {
                    if (codigo_nat[posicao] == "'")
                    {
                        cod_funcao.Add("\"");
                        posicao++;
                        while (codigo_nat[posicao] != "'")
                        {
                            cod_funcao.Add(codigo_nat[posicao]);
                            posicao++;
                        }
                        posicao++;
                        cod_funcao.Add("\"");
                    }
                    else
                    {
                        eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao - 1]);
                        if ((cont == 0 && codigo_nat[posicao] != ")" && codigo_nat[posicao] != "(" && codigo_nat[posicao] != "="
                            && codigo_nat[posicao] != "EQ" && codigo_nat[posicao] != "NE" && codigo_nat[posicao] != "GT"
                            && codigo_nat[posicao] != "LE" && codigo_nat[posicao] != "GE" && codigo_nat[posicao] != "LT"
                            && codigo_nat[posicao] != "TRUE" && codigo_nat[posicao] != "FALSE" && !eh_numero)
                            && (codigo_nat[posicao - 1] == "TRUE" || codigo_nat[posicao - 1] != "FALSE" || eh_variavel))
                        {
                            cont++;
                            cod_funcao.Add(")");
                            cod_funcao.Add("{");
                        }
                        if (eh_nat_reservadas)
                        {
                            if (this.nat_reservadas[codigo_nat[posicao]] == "")
                            {
                                switch (codigo_nat[posicao])
                                {
                                    case "IF":
                                        migra_IF(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "MOVE":
                                        migra_MOVE(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "PERFORM":
                                        migra_PERFORM(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "WRITE":
                                        migra_WRITE(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "FOR":
                                        migra_FOR(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "REPEAT":
                                        migra_REPEAT(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "INPUT":
                                        migra_INPUT(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "DECIDE":
                                        migra_DECIDE(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                }
                            }
                            else
                            {
                                if (codigo_nat[posicao] == "ELSE")
                                {
                                    cod_funcao.Add("}");
                                    cod_funcao.Add(this.nat_reservadas[codigo_nat[posicao]]);
                                    cod_funcao.Add("{");
                                }
                                else
                                {
                                    cod_funcao.Add(this.nat_reservadas[codigo_nat[posicao]]);
                                }
                                if (codigo_nat[posicao] == "END-IF")
                                {
                                    testa_final = true;
                                }
                                posicao++;
                            }
                        }
                        else
                        {
                            if (eh_numero)
                            {
                                cod_funcao.Add(codigo_nat[posicao]);
                                posicao++;
                            }
                            else
                            {
                                if (codigo_nat[posicao] == ")")
                                {
                                    cod_funcao.Add(")");
                                    posicao++;
                                }
                                if (codigo_nat[posicao] == "(")
                                {
                                    cod_funcao.Add("(");
                                    posicao++;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void migra_FOR(ref int posicao, string[] codigo_nat, ArrayList cod_funcao)
        {
            bool eh_variavel, eh_vetor, eh_nat_reservadas, eh_funcao, eh_numero;
            int operador = 0, ax_ind = 0;
            string aux_operando = "";

            bool testa_final = false;
            cod_funcao.Add("for");
            cod_funcao.Add("(");
            posicao++;

            while (testa_final != true)
            {
                eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]);
                eh_nat_reservadas = this.nat_reservadas.ContainsKey(codigo_nat[posicao]);
                eh_funcao = this.funcoes.ContainsKey(codigo_nat[posicao]);
                eh_numero = this.valida_Numero(codigo_nat[posicao]);

                if (eh_variavel)
                {
                    if (eh_vetor)
                    {
                        if (codigo_nat[posicao + variaveis_vet[codigo_nat[posicao]][1]] != ":=")
                        {
                            migra_VETOR(ref posicao, codigo_nat, cod_funcao);
                            posicao++;
                        }
                        else
                        {
                            migra_MATH(ref posicao, codigo_nat, cod_funcao);
                        }
                    }
                    else
                    {
                        if (codigo_nat[posicao + 1] != ":=")
                        {
                            if (operador == 3)
                            {
                                cod_funcao.Add("+=");
                            }
                            cod_funcao.Add(this.variaveis[codigo_nat[posicao]][2]);
                            if (operador == 0)
                            {
                                aux_operando = this.variaveis[codigo_nat[posicao]][2].ToString();
                                cod_funcao.Add("=");
                                operador++;
                            }
                            else
                            {
                                if (operador == 1 || operador == 2)
                                {
                                    cod_funcao.Add(";" + aux_operando);
                                    operador++;
                                }
                                else
                                {
                                    if (operador == 3)
                                    {
                                        cod_funcao.Add(")");
                                        cod_funcao.Add("{");
                                        operador++;
                                    }
                                }
                            }
                            posicao++;
                        }
                        else
                        {
                            if (operador == 3)
                            {
                                cod_funcao.Add("+=");
                                cod_funcao.Add("1");
                                cod_funcao.Add(")");
                                cod_funcao.Add("{");
                                operador++;
                            }
                            migra_MATH(ref posicao, codigo_nat, cod_funcao);
                        }
                    }
                }
                else
                {
                    if (eh_numero)
                    {
                        if (operador == 3)
                        {
                            cod_funcao.Add("+=");
                        }
                        if (operador == 1)
                        {
                            ax_ind = Convert.ToInt32(codigo_nat[posicao]) - 1;
                            cod_funcao.Add(Convert.ToString(ax_ind));
                        }
                        else
                        {
                            cod_funcao.Add(codigo_nat[posicao]);
                        }
                        if (operador == 1 || operador == 2)
                        {
                            cod_funcao.Add(";" + aux_operando);
                            operador++;
                        }
                        else
                        {
                            if (operador == 3)
                            {
                                cod_funcao.Add(")");
                                cod_funcao.Add("{");
                                operador++;
                            }
                        }
                        posicao++;
                    }
                    else
                    {
                        if (eh_nat_reservadas)
                        {
                            if (codigo_nat[posicao] == "TO")
                            {
                                cod_funcao.Add("!=");
                                posicao++;
                            }
                            else
                            {
                                if (operador == 3)
                                {
                                    cod_funcao.Add("+");
                                    cod_funcao.Add("1");
                                    cod_funcao.Add(")");
                                    cod_funcao.Add("{");
                                    operador++;
                                    posicao++;
                                }
                                if (this.nat_reservadas[codigo_nat[posicao]] == "")
                                {
                                    switch (codigo_nat[posicao])
                                    {
                                        case "IF":
                                            migra_IF(ref posicao, codigo_nat, cod_funcao);
                                            break;
                                        case "MOVE":
                                            migra_MOVE(ref posicao, codigo_nat, cod_funcao);
                                            break;
                                        case "PERFORM":
                                            migra_PERFORM(ref posicao, codigo_nat, cod_funcao);
                                            break;
                                        case "WRITE":
                                            migra_WRITE(ref posicao, codigo_nat, cod_funcao);
                                            break;
                                        case "FOR":
                                            migra_FOR(ref posicao, codigo_nat, cod_funcao);
                                            break;
                                        case "REPEAT":
                                            migra_REPEAT(ref posicao, codigo_nat, cod_funcao);
                                            break;
                                        case "INPUT":
                                            migra_INPUT(ref posicao, codigo_nat, cod_funcao);
                                            break;
                                        case "DECIDE":
                                            migra_DECIDE(ref posicao, codigo_nat, cod_funcao);
                                            break;
                                    }
                                }
                                else
                                {
                                    cod_funcao.Add(this.nat_reservadas[codigo_nat[posicao]]);
                                    if (codigo_nat[posicao] == "END-FOR")
                                    {
                                        testa_final = true;
                                        posicao++;
                                    }
                                }
                            }
                        }
                    }
                }  
            }
        }

        public void migra_REPEAT(ref int posicao, string[] codigo_nat, ArrayList cod_funcao)
        {
            bool eh_variavel, eh_vetor, eh_nat_reservadas, eh_funcao, eh_numero;
            int cont = 0;
            bool testa_final = false;

            posicao++;

            while (testa_final != true)
            {
                if (codigo_nat[posicao] == "WHILE")
                {
                    cod_funcao.Add("while");
                    posicao++;
                    if (codigo_nat[posicao] != "(")
                    {
                        cod_funcao.Add("(");
                    }
                }

                eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]);
                eh_nat_reservadas = this.nat_reservadas.ContainsKey(codigo_nat[posicao]);
                eh_funcao = this.funcoes.ContainsKey(codigo_nat[posicao]);
                eh_numero = this.valida_Numero(codigo_nat[posicao]);

                
                if (eh_variavel)
                {
                    if (eh_vetor)
                    {
                        if (codigo_nat[posicao + variaveis_vet[codigo_nat[posicao]][1]] != ":=")
                        {
                            migra_VETOR(ref posicao, codigo_nat, cod_funcao);
                            posicao++;
                        }
                        else
                        {
                            migra_MATH(ref posicao, codigo_nat, cod_funcao);
                        }
                    }
                    else
                    {
                        if (codigo_nat[posicao + 1] != ":=")
                        {
                            cod_funcao.Add(this.variaveis[codigo_nat[posicao]][2]);
                            posicao++;
                        }
                        else
                        {
                            migra_MATH(ref posicao, codigo_nat, cod_funcao);
                        }
                    }
                }
                else
                {
                    if (codigo_nat[posicao] == "'")
                    {
                        cod_funcao.Add("\"");
                        posicao++;
                        while (codigo_nat[posicao] != "'")
                        {
                            cod_funcao.Add(codigo_nat[posicao]);
                            posicao++;
                        }
                        posicao++;
                        cod_funcao.Add("\"");
                    }
                    else
                    {
                        eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao - 1]);
                        if ((cont == 0 && codigo_nat[posicao] != ")" && codigo_nat[posicao] != "(" && codigo_nat[posicao] != "="
                            && codigo_nat[posicao] != "EQ" && codigo_nat[posicao] != "NE" && codigo_nat[posicao] != "GT"
                            && codigo_nat[posicao] != "LE" && codigo_nat[posicao] != "GE" && codigo_nat[posicao] != "LT"
                            && codigo_nat[posicao] != "TRUE" && codigo_nat[posicao] != "FALSE" && !eh_numero)
                            && (codigo_nat[posicao - 1] == "TRUE" || codigo_nat[posicao - 1] != "FALSE" || eh_variavel))
                        {
                            cont++;
                            cod_funcao.Add(")");
                            cod_funcao.Add("{");
                        }
                        if (eh_nat_reservadas)
                        {
                            if (this.nat_reservadas[codigo_nat[posicao]] == "")
                            {
                                switch (codigo_nat[posicao])
                                {
                                    case "IF":
                                        migra_IF(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "MOVE":
                                        migra_MOVE(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "PERFORM":
                                        migra_PERFORM(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "WRITE":
                                        migra_WRITE(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "FOR":
                                        migra_FOR(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "REPEAT":
                                        migra_REPEAT(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "INPUT":
                                        migra_INPUT(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "DECIDE":
                                        migra_DECIDE(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                }
                            }
                            else
                            {
                                cod_funcao.Add(this.nat_reservadas[codigo_nat[posicao]]);
                                if (codigo_nat[posicao] == "END-REPEAT")
                                {
                                    testa_final = true;
                                }
                                posicao++;
                            }
                        }
                        else
                        {
                            if (eh_numero)
                            {
                                cod_funcao.Add(codigo_nat[posicao]);
                                posicao++;
                            }
                            else
                            {
                                if (codigo_nat[posicao] == ")")
                                {
                                    cod_funcao.Add(")");
                                    posicao++;
                                }
                                if (codigo_nat[posicao] == "(")
                                {
                                    cod_funcao.Add("(");
                                    posicao++;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void migra_DECIDE(ref int posicao, string[] codigo_nat, ArrayList cod_funcao)
        {
            bool eh_variavel, eh_vetor, eh_nat_reservadas, eh_funcao, eh_numero;
            bool testa_final = false;
            int cont_value = 0;
            
            cod_funcao.Add("switch");
            cod_funcao.Add("(");
            posicao++;

            eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
            while (!eh_variavel)
            {
                posicao++;
                eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);         
            }
            eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]);

            if (eh_vetor)
            {
                migra_VETOR(ref posicao, codigo_nat, cod_funcao);
                posicao++;
            }
            else
            {
                cod_funcao.Add(this.variaveis[codigo_nat[posicao]][2]);
                posicao++;
            }
            cod_funcao.Add(")");
            cod_funcao.Add("{");

            while (testa_final != true)
            {
                eh_variavel = this.variaveis.ContainsKey(codigo_nat[posicao]);
                eh_vetor = this.variaveis_vet.ContainsKey(codigo_nat[posicao]);
                eh_nat_reservadas = this.nat_reservadas.ContainsKey(codigo_nat[posicao]);
                eh_funcao = this.funcoes.ContainsKey(codigo_nat[posicao]);
                eh_numero = this.valida_Numero(codigo_nat[posicao]);

                if (eh_nat_reservadas)
                {
                    if (codigo_nat[posicao] == "VALUE")
                    {
                        if (cont_value > 0)
                        {
                            cod_funcao.Add("break");
                            cod_funcao.Add(";");
                        }
                        cont_value++;
                        posicao++;
                        cod_funcao.Add("case");
                        eh_numero = this.valida_Numero(codigo_nat[posicao]);
                        if (eh_numero)
                        {
                            cod_funcao.Add(codigo_nat[posicao]);
                            cod_funcao.Add(":");
                        }
                        posicao++;
                    }
                    else
                    {
                        if (codigo_nat[posicao] == "NONE")
                        {
                            cod_funcao.Add("break");
                            cod_funcao.Add(";");
                            cod_funcao.Add("default");
                            cod_funcao.Add(":");
                            posicao++;
                        }
                        else
                        {
                            if (this.nat_reservadas[codigo_nat[posicao]] == "")
                            {
                                switch (codigo_nat[posicao])
                                {
                                    case "IF":
                                        migra_IF(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "MOVE":
                                        migra_MOVE(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "PERFORM":
                                        migra_PERFORM(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "WRITE":
                                        migra_WRITE(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "FOR":
                                        migra_FOR(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "REPEAT":
                                        migra_REPEAT(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "INPUT":
                                        migra_INPUT(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                    case "DECIDE":
                                        migra_DECIDE(ref posicao, codigo_nat, cod_funcao);
                                        break;
                                }
                            }
                            else
                            {
                                if (codigo_nat[posicao] == "END-DECIDE")
                                {
                                    cod_funcao.Add("break");
                                    cod_funcao.Add(";");
                                    cod_funcao.Add(this.nat_reservadas[codigo_nat[posicao]]);
                                    testa_final = true;
                                }
                                posicao++;
                            }
                        }
                    }
                }

            }

        }

        public void imprimir_Variaveis()
        {
            int ind_pos_classe, ind_pos_chave;
            bool ex_variavel;

            foreach (KeyValuePair<int, List<string>> classe in this.classes_saida)
            {
                ind_pos_classe = this.saida_csharp.IndexOf("class Classe_" + classe.Key);
                ind_pos_chave = this.saida_csharp.IndexOf("{", ind_pos_classe);
                ind_pos_chave += 1;

                this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, "\n" + " // Final_declação_variaveis_classe_" + classe.Key + "\n");

                for (int i = 0; i < classe.Value.Count; i++)
                {
                    ex_variavel = this.variaveis.ContainsKey(classe.Value[i]);
                    if (ex_variavel)
                    {
                        for (int y = this.variaveis[classe.Value[i]].Count - 1; y > -1; y--)
                        {
                            this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, this.variaveis[classe.Value[i]][y].ToString() + " ");
                        }
                        this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, "\n" + this.identacao);
                    }
                }
            }
        }

        public void imprimir_Get_Set_Variaaveis()
        {
            int ind_pos_classe, ind_pos_chave;
            bool ex_variavel, ex_vetor;

            foreach (KeyValuePair<int, List<string>> classe in this.classes_saida)
            {
                for (int i = 0; i < classe.Value.Count; i++)
                {
                    ex_variavel = this.variaveis.ContainsKey(classe.Value[i]);
                    ex_vetor = this.variaveis_vet.ContainsKey(classe.Value[i]);
                    if (ex_variavel && !ex_vetor)
                    {
                        ind_pos_classe = this.saida_csharp.IndexOf("class Classe_" + classe.Key);
                        ind_pos_chave = this.saida_csharp.IndexOf(" // Final_da_classe_" + classe.Key, ind_pos_classe);
                        ind_pos_chave -= 1;
                        this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, "\n" + this.identacao + "public " + this.variaveis[classe.Value[i]][1].ToString() + " get_Set_" + this.variaveis[classe.Value[i]][2].ToString());
                        ind_pos_classe = this.saida_csharp.IndexOf("class Classe_" + classe.Key);
                        ind_pos_chave = this.saida_csharp.IndexOf(" // Final_da_classe_" + classe.Key, ind_pos_classe);
                        ind_pos_chave -= 1;
                        this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, "\n" + this.identacao + "{");
                        this.adicionar_Identacao();
                        ind_pos_classe = this.saida_csharp.IndexOf("class Classe_" + classe.Key);
                        ind_pos_chave = this.saida_csharp.IndexOf(" // Final_da_classe_" + classe.Key, ind_pos_classe);
                        ind_pos_chave -= 1;
                        this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, "\n" + this.identacao + "get { return " + this.variaveis[classe.Value[i]][2].ToString() + "; }");
                        ind_pos_classe = this.saida_csharp.IndexOf("class Classe_" + classe.Key);
                        ind_pos_chave = this.saida_csharp.IndexOf(" // Final_da_classe_" + classe.Key, ind_pos_classe);
                        ind_pos_chave -= 1;
                        this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, "\n" + this.identacao + "set { " + this.variaveis[classe.Value[i]][2].ToString() + " = value; }");
                        this.remover_Identacao();
                        ind_pos_classe = this.saida_csharp.IndexOf("class Classe_" + classe.Key);
                        ind_pos_chave = this.saida_csharp.IndexOf(" // Final_da_classe_" + classe.Key, ind_pos_classe);
                        ind_pos_chave -= 1;
                        this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, "\n" + this.identacao + "} \n");
                    }
                }
            }

        }

        public void imprimir_Funcoes()
        {
            int ind_pos_classe, ind_pos_chave;
            bool ex_funcao;

            foreach (KeyValuePair<int, List<string>> classe in this.classes_saida)
            {
                ind_pos_classe = this.saida_csharp.IndexOf("class Classe_" + classe.Key);
                ind_pos_chave = this.saida_csharp.IndexOf(" // Final_da_classe_" + classe.Key, ind_pos_classe);          
                ind_pos_chave -= 1;

                for (int i = 0; i < classe.Value.Count; i++)
                {
                    ex_funcao = this.funcoes.ContainsKey(classe.Value[i]);
                    if (ex_funcao)
                    {
                        for (int y = 0; y < this.funcoes[classe.Value[i]].Count; y++)
                        {
                            ind_pos_classe = this.saida_csharp.IndexOf("class Classe_" + classe.Key);
                            ind_pos_chave = this.saida_csharp.IndexOf(" // Final_da_classe_" + classe.Key, ind_pos_classe);
                            ind_pos_chave -= 1;

                            if (this.funcoes[classe.Value[i]][y].ToString() == "public" || this.funcoes[classe.Value[i]][y].ToString() == "static" ||
                                this.funcoes[classe.Value[i]][y].ToString() == "}" || this.funcoes[classe.Value[i]][y].ToString() == "{" ||
                                this.funcoes[classe.Value[i]][y].ToString() == ";" || this.funcoes[classe.Value[i]][y].ToString() == ":")
                            {
                                if (this.funcoes[classe.Value[i]][y].ToString() == "}")
                                {
                                    this.remover_Identacao();
                                    this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, "\n" + this.identacao + this.funcoes[classe.Value[i]][y].ToString() + "\n");
                                    ind_pos_classe = this.saida_csharp.IndexOf("class Classe_" + classe.Key);
                                    ind_pos_chave = this.saida_csharp.IndexOf(" // Final_da_classe_" + classe.Key, ind_pos_classe);
                                    ind_pos_chave -= 1;
                                    this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, this.identacao);
                                }
                                else
                                {
                                    if (this.funcoes[classe.Value[i]][y].ToString() == "{")
                                    {
                                        this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, "\n" + this.identacao + this.funcoes[classe.Value[i]][y].ToString() + "\n");
                                        this.adicionar_Identacao();
                                        ind_pos_classe = this.saida_csharp.IndexOf("class Classe_" + classe.Key);
                                        ind_pos_chave = this.saida_csharp.IndexOf(" // Final_da_classe_" + classe.Key, ind_pos_classe);
                                        ind_pos_chave -= 1;
                                        this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, this.identacao);
                                    }
                                    else
                                    {
                                        if (this.funcoes[classe.Value[i]][y].ToString() == ";" || this.funcoes[classe.Value[i]][y].ToString() == ":")
                                        {
                                            this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, this.funcoes[classe.Value[i]][y].ToString() + "\n" + this.identacao);
                                        }
                                        else
                                        {
                                            this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, "\n" + this.identacao + this.funcoes[classe.Value[i]][y].ToString() + " ");
                                        }
                                    }
                                }   
                            }
                            else
                            {
                                this.saida_csharp = this.saida_csharp.Insert(ind_pos_chave, this.funcoes[classe.Value[i]][y].ToString() + " ");
                            }
                       }
                    }
                }
            }
        }
    }
}