# superdigital-bank# Super Digital - Bank

Postman para teste:
https://www.getpostman.com/collections/0753a8e6d45f3dd9b822

* .Net Core 2.0
* FluentValidation
* Dapper
* DDD
* OAuth2
* SQL Server

Usuários para teste:

|Nome| Email  | Senha |
|-| ------------- | ------------- |
|Acaciano Neves| acaciano.neves@gmail.com  | aa@10  |
|Amanda Ellen| aednamanda@gmail.com  | aa@10  |

URL API local (oAuth2): http://localhost:5001/token

Respostas:

1. DDD é uma prática de desenvolvimento de software onde o foco principal é o negócio (Dominio), DDD tem diversas práticas, padrões e abordagens para um desenvolvimento de software de aplicações complexas, visando com isso uma forma de melhorar a comunicação entre desenvolvedores, analistas de negócios, clientes, etc. Assim fazendo com o que o software por si só seja autodescrito, todos entendem perfeitamente os termos, o codigo expressa exatamente o que a aplicação foi criada para fazer, divide as resposabilidades em contextos menores para diminuir a complexidade, cada contexto deve fazer exatamente o que ele se destina a resolver.

2. Arquitetura baseada em microserviços é definida por separar os módulos do sistema em pequenos serviços, ou seja que faça somente uma coisa, ela funciona com a quebra de uma solução em varias partes pequenas que são independentes, cada microserviço pode ter sua propria técnologia o quer ajuda muito em ter uma tecnologia otima para aquele determinado problema a ser resolvido. A grande vantagem de trabalhar com esse modelo é escalabilidade, é possivel escalar um ponto especifico que necessita por algum motivo sazional naquele momento precisar de mais recursos.

3. A diferença entre comunicação síncrona e comunicação assíncrona é que a primeira é utilizada para execuções onde o resultado é esperado de forma urgente. Já a segunda é utilizada para comunicações que podem ser respondidas tempos após a execução e enquanto esse retorno não é recebido, o processo pode executar outras tarefas.
Comunicação sincrona é mais indicada em cenários em que você precisa da resposta imediatamente, como algo em real time onde não pode haver uma inconsitencia eventual, já o assincrono seria para cenários em que você não necessita exatamente de uma resposta imediata uma compra com cartão de crédito por exemplo a compra é efetuada e a resposta da aprovação não necessariamente vem no mesmo momento, o usuário é notificado com status final da compra.
