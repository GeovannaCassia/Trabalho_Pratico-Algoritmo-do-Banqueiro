# 💰Algoritmo do Banqueiro - Implementação em C#

📌 DESCRIÇÃO

Este projeto foi desenvolvido como trabalho prático da disciplina de Sistemas Operacionais da PUC Minas. A aplicação implementa o Algoritmo do Banqueiro utilizando programação multithreaded em C#, com o objetivo de simular a alocação segura de recursos e evitar situações de deadlock.

O sistema modela múltiplos clientes que realizam requisições e liberações de recursos de forma concorrente, sendo gerenciados por uma entidade central (o banqueiro), responsável por garantir que o sistema permaneça sempre em um estado seguro.
_________________________________________________________________________________________________________________________________________________
⚙️ TECNOLOGIAS UTILIZADAS 

C#
.NET 10
Programação concorrente com Threads
▶️ Como Executar
🔹 1. Clonar o repositório
git clone https://github.com/seu-usuario/seu-repositorio.git
🔹 2. Acessar a pasta do projeto
cd nome-do-projeto
🔹 3. Executar o programa
dotnet run 10 5 7

📌 Os valores representam a quantidade de recursos disponíveis para cada tipo.

Caso nenhum parâmetro seja informado, o sistema utilizará valores padrão definidos no código.

🧠 Funcionamento
Cada cliente é representado por uma thread independente
Clientes geram requisições aleatórias de recursos
O algoritmo do banqueiro verifica se a alocação mantém o sistema em estado seguro
Se o estado for seguro → os recursos são concedidos
Caso contrário → a requisição é negada
🔒 Controle de Concorrência

Para evitar condições de corrida (race conditions), foi utilizado o mecanismo de exclusão mútua (lock) da linguagem C#, garantindo que apenas uma thread por vez acesse as estruturas de dados compartilhadas.

📁 Estrutura do Projeto
Program.cs → Inicialização do sistema
Customer.cs → Representação dos clientes (threads)
Banker.cs → Implementação do algoritmo do banqueiro
📚 Referência

SILBERSCHATZ, Abraham; GALVIN, Peter B.; GAGNE, Greg. Fundamentos de Sistemas Operacionais. 9. ed. Rio de Janeiro: LTC, 2015.
