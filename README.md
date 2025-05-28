# Desafio Técnico Objective

Este projeto é uma solução para o desafio técnico proposto pela Objective, desenvolvido em .NET 9.

## Tecnologias Utilizadas

- .NET 9
- C#
- Visual Studio

## Como Executar

1. Clone o repositório:
2. Abra a solução no Visual Studio.
3. Restaure os pacotes NuGet.
4. Execute o projeto pressionando `F5` ou utilizando o menu "Iniciar Depuração".

## Estrutura do Projeto

- `DesafioTecnicoObjective/` - Contém o código-fonte principal do desafio.
- `README.md` - Este arquivo de documentação.

## Testes

Para rodar os testes, utilize o Test Explorer do Visual Studio ou execute o comando.

## Contato

Dúvidas ou sugestões? Entre em contato pelo e-mail: guilherme.jannotti@gmail.com


## Observação sobre o uso de float nos valores monetários 

O enunciado do desafio solicita explicitamente o uso do tipo float para representar o saldo inicial das contas.

Embora tenha seguido essa instrução fielmente, é importante destacar que o tipo float não é recomendado para operações financeiras em aplicações reais, 
devido à sua natureza binária de representação e à consequente imprecisão em cálculos decimais, o que pode causar erros de arredondamento acumulativos.

Em ambientes de produção, o tipo mais apropriado para representar valores monetários em C# é o decimal, que oferece maior precisão e segurança para cálculos financeiros.

Outra abordagem recomendada é usar int para armazenar valores em centavos (por exemplo, 12345 para representar R$123,45), principalmente em sistemas de alto desempenho, 
como gateways de pagamento ou bancos. Essa técnica elimina completamente problemas de precisão, já que não há frações, e pode ser mais eficiente para o processador, 
pois operações com inteiros são mais rápidas que com decimais. Mas exige conversão manual para exibição.

Apesar disso, optei por manter o uso de float para garantir conformidade com o enunciado do desafio, considerando também a possibilidade de existirem validadores automáticos que esperam esse tipo específico.
