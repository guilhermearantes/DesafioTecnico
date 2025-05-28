# Desafio T�cnico Objective

Este projeto � uma solu��o para o desafio t�cnico proposto pela Objective, desenvolvido em .NET 9.

## Tecnologias Utilizadas

- .NET 9
- C#
- Visual Studio

## Como Executar

1. Clone o reposit�rio:
2. Abra a solu��o no Visual Studio.
3. Restaure os pacotes NuGet.
4. Execute o projeto pressionando `F5` ou utilizando o menu "Iniciar Depura��o".

## Estrutura do Projeto

- `DesafioTecnicoObjective/` - Cont�m o c�digo-fonte principal do desafio.
- `README.md` - Este arquivo de documenta��o.

## Testes

Para rodar os testes, utilize o Test Explorer do Visual Studio ou execute o comando.

## Contato

D�vidas ou sugest�es? Entre em contato pelo e-mail: guilherme.jannotti@gmail.com


## Observa��o sobre o uso de float nos valores monet�rios 

O enunciado do desafio solicita explicitamente o uso do tipo float para representar o saldo inicial das contas.

Embora tenha seguido essa instru��o fielmente, � importante destacar que o tipo float n�o � recomendado para opera��es financeiras em aplica��es reais, 
devido � sua natureza bin�ria de representa��o e � consequente imprecis�o em c�lculos decimais, o que pode causar erros de arredondamento acumulativos.

Em ambientes de produ��o, o tipo mais apropriado para representar valores monet�rios em C# � o decimal, que oferece maior precis�o e seguran�a para c�lculos financeiros.

Outra abordagem recomendada � usar int para armazenar valores em centavos (por exemplo, 12345 para representar R$123,45), principalmente em sistemas de alto desempenho, 
como gateways de pagamento ou bancos. Essa t�cnica elimina completamente problemas de precis�o, j� que n�o h� fra��es, e pode ser mais eficiente para o processador, 
pois opera��es com inteiros s�o mais r�pidas que com decimais. Mas exige convers�o manual para exibi��o.

Apesar disso, optei por manter o uso de float para garantir conformidade com o enunciado do desafio, considerando tamb�m a possibilidade de existirem validadores autom�ticos que esperam esse tipo espec�fico.