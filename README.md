# Teste Técnico B3
Este projeto foi desenvolvido com intuito de atender a um teste técnico da B3 junto a Genesis Consulting seguindo as orientações abaixo.

## Contexto do case
  1. Criação de uma tela web onde possamos informar um valor
monetário positivo, e um prazo em meses maior que 1(um) para resgate
da aplicação. Após solicitar o cálculo do investimento, a tela deve
apresentar o resultado bruto e o resultado líquido do investimento.

  2. Criação de uma Web API que receba os dados informados
no item 1

      a. Para o cálculo do CDB, deve-se utilizar a fórmula VF = VI x [1 +
(CDI x TB)] onde:

          i. VF é o valor final;
     
          ii. VI é o valor inicial;
     
          iii. CDI é o valor dessa taxa no último mês;
     
          iv. TB é quanto o banco paga sobre o CDI;
     
          Nota: A fórmula calcula somente o valor de um mês. Ou seja, os
          rendimentos de cada mês devem ser utilizados para calcular o mês
          seguinte
     
     b. Para medida do Exercício considerar os valores abaixo como fixos:
          i. TB – 108%
          ii. CDI – 0,9%

  3. INFORMAÇÃO PÚBLICA
     
    c. Para cálculo do imposto utilizar a seguinte tabela:
    
          i. Até 06 meses: 22,5%
          ii. Até 12 meses: 20%
          iii. Até 24 meses 17,5%
          iv. Acima de 24 meses 15%


## Implementação

### Banco de dados
Para o case foi criado um banco de dados relacional em Microsoft SQL Server chamado <b>TesteTecnicoB3</b>. 
Para o banco foram criadas as seguintes Entidades(tabelas):

#### Titulo
Tabela que armazena os titulos para investimento, nela estará dados como o nome do titulo, tipo, se ele é um titulo pós-fixado ou pré-fixado, no caso de pós-fixado saber qual indice do mesmo e qual sua taxa de rendimento(se for pré-fixado será esta a taxa final, caso seja pós-fixado este será a porcentagem do indice utilizado).

#### TituloTipo
Tabela que armazena o titulo de titulo.<br>
  Exemplo: CDB, LCI, Tesouro Direto e etc...
  
#### IndiceTipo
Tabela utilizada para armazenar os indices o qual os titulos pós-fixados utilizarão e qual a taxa atual do mesmo.<br>
  Exemplo: SELIC, CDI, IPCA e etc...<br><br>

  <i><b>Obs:</b> Esta tabela existe uma coluna data DataAtualização, o qual foi pensado um cenário em que o Banco Central envia requisições para atualização das taxas dos mesmos.</i>

#### DescontoImpostoRenda 
Tabela utilizada para armazenar os percentuais de desconto do Imposto de Renda baseado no tempo em meses o qual o titulo será investido, para isso será feito um filtro utilizado as colunas QtdeMesesInicio e QtdeMesesFim e descobrido assim o Percentual respectivo.



### Back-end
Para o back-end foi criada uma solucação em WebAPi utilizando .Net 6, utilizando os conceitos de SOLID e arquitetura limpa, o qual temos os seguintes End-Points atualmente:

#### APIS

##### (GET) /titulos 
  Api para trazer uma lista completa de titulos cadastrados no BD
  <i><b>Obs:</b> Esta API não está sendo utilizado pelo Front-end, pois realizada uma melhoria de página de titulo para Renda-fixa.</i>
  
##### (GET) /titulos/{id}
  Api que traz os dados de um titulo especifico.
  <i><b>Obs:</b> Esta API não está sendo utilizado pelo Front-end, pois o titulo quando é selecionado em tela está sendo reaproveitado os dados da tela anterior, não necessitando fazer uma nova consulta em banco de dados.</i>

##### (GET) /titulos/renda-fixa
  Api para trazer uma lista de titulos classificados por Renda Fixa(de acordo com o Tipo do mesmo).

##### (GET) /Titulo/simulacao/{id}/{valorInicial}/{valorAporteMensal}/{qtdeMesesInvestimento}
  Api que fará a simulação do titulo escolhido em tela, o qual receberá o Valor Inicial investido, o valor de aporte mensal(caso exista) e a quantidade de meses que o usuário pretende investir este valor.<br>
  O Resultado para o mesmo será uma SimulacaoViewModel, o qual trará dados como o Valor total Investido, Valor Final Bruto, Valor de Rendimento Total, Desconto de I.R. e o Valor Final Líquido.



### Front-end
Foi realizada a construção de uma aplicação web em Angular CLI o qual teremos as seguintes web pages:

##### Pages

##### Home
Página de boas vindas ao projeto.

##### Renda-Fixa
Página que conterá a lista de títulos de renda fixa, o qual utilizará a api <b> /titulos/renda-fixa </b> citada na seção de Apis do back-end. 
Será possível realizar a simulação de um título selecionado, o qual será exibido um formulario pedindo o Valor Incial de Investimento, Valor de Aporte Mensal e Quantidade de Meses.
O resultado da simulação será exibido ao lado, mostrando o Valor Bruto, Valor Total Investido, Valor Rendimentos, Desconto(I.R.) e Valor Líquido obtidos em reais.



## Execução

Para executar o projeto faremos o mesmo em duas etapas: Back-end e Front-end.

### Back-end

1 - Abrir o projeto no Visual Studio.

2 - Selecionar o perfil <b>docker-compose</b> e executar o projeto.


### Front-end

1 - Precisaremos iniciar um prompt de comando dentro da pasta do front-end: <b><i>./B3.Application.FrontEnd/</i></b>.

2 - Dentro do prompt de comando executar o seguinte comando:

      ng serve --open


