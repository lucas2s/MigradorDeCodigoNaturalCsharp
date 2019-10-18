# MigradorDeCodigoNaturalC-

Trabalho de conclusão de curso da graduação.

Ferramenta capaz de transformar códigos fontes na linguagem Natural desenvolvidos no paradigma estruturado, para códigos fontes na linguagem C# desenvolvidos no paradigma orientado a objetos.

Algoritmo para migração do modelo estruturado para orientado a objeitos:
Encontrar elementos do código fonte (variáveis e funções) e seus relacionamentos.
Ex:  
F1 = {F1, V1, V2, V4}
F2 = {F2, V1, V3}
V1 = {V1, V2, V3, V4,F1, F2}
V2 = {V2, V1, V4,  F1}
V3 = {V3, V1,  F2}
V4 = {V4, V1, V2, F1}


Calculo da Distância entre os elementos .

Onde, A é um elemento e B é outro elemento diferente de A.
Ex:  
DV1 x F1 = 1 – (4 / 6) = 1 -  0.666666 = 0.333333
DV4 x F2 = 1 – (1 / 6) = 1 -  0.166666 = 0.833333

Quanto menor a distância, maior é o relacionamento entre os elementos no código fonte.


Referências

DINESHKUMAR V; DEEPIKA J. Code to Design Migration from Structured to Object Oriented Paradigm. Dezembro 2011. Disponível em: http://esjournals.org/journaloftechnology/archive/vol1no8/vol1no8_4.pdf. Acesso em 15 abril 2012.

TEREKHOV, Andrey A.; VERHOEF, Chris. The Realities of Language Conversions. Dezembro 2000. Disponível em: http://www.cs.vu.nl/~x/cnv/s6.pdf. Acesso em 08 Abril 2012.
