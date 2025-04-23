# TaskManager API

**TaskManager API** é uma REST API que permita aos usuários organizar e monitorar suas tarefas diárias, bem como colaborar com colegas de equipe.

This project follows Clean Architecture principles, SOLID design, and modern best practices.
Este projeto segue boas práticas de SOLID, CLEAN ARCHITETUCRE e alguns padrões de projeto.

---

##  Endpoints disponíveis

- Projects
  - **GetProjectsByUser** - listar todos os projetos do usuário
  - **GetTasksByProject** - visualizar todas as tarefas de um projeto específico
  - **NewProject** - criar um novo projeto
  - **DeleteProject** - criar um novo projeto

- Tasks
  - **NewTask** - adicionar uma nova tarefa a um projeto
  - **UpdateTask** - atualizar o status ou detalhes de uma tarefa
  - **DeleteTask** - remover uma tarefa de um projeto
  - **GetReportTasksLastDays** - retorna o número médio de tarefas concluídas por usuário nos últimos 30 dias de um usuário (WIP)

---

## 🧱 Technologies

- .NET 9
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Swagger
- xUnit
- Docker

---

## 📦 Project Structure

```
TaskManager/
├── docker-compose      # Configurações DOCKER do Projeto
├── API                 # Controllers, middleware, Variaveis de ambiente
├── Application         # ConfigureServices, Services, UseCases
├── Domain              # DTOs, Entities, Enums e Models
├── Infrastructure      # Interfaces de Repositories e Services 
├── Persistence         # DbContext, Migrations e Repositories
├── Tests               # Unit tests
```
<BR>

## 🚀 COMO EXECUTAR O PROJETO?
### PRE-REQUISITOS
- **Para Windows**
  - WSL2
  - Docker Desktop
  - SDK .NET 9.0
  - Visual Studio 2022

### EXECUTANDO A API PELA PRIMEIRA VEZ
1) Antes de abrir o arquivo do Projeto **"TaskManager.sln"**, deixe o DOCKER DESKTOP rodando no seu computador.
2) Abra o Visual Studio 2022, clique no Botão "Open a Project Soluction"
3) Em seguida, abre o arquivo "TaskManager.sln"
4) Após o Projeto abrir no Visual Studio 2022, aperte as Teclas **CTRL** **+** **'** para abrir o "Developer Command Prompt"
5) Digite: **cd TaskManager.Persistence** para acessar o diretório do Projeto de configuração do Banco de Dados
6) Depois, digite: **dotnet ef migrations add InitialCreate --startup-project ../TaskManager.API**
4) Clique em "Soluction Explorer" e, em seguida, aperte a tecla F5


## Arquivo de Cobertura dos Tests
Abre em qualquer navegador
``` 
TaskManager/TaskManager.UnitTests/TestResults/CoverageReport/index.html 
```
----
<br>

### Caso precise extrair novamente a cobertura dos Testes Unitários
A política de execução do PowerShell impede a execução de scripts não assinados digitalmente. Para resolver isso, você pode alterar temporariamente a política de execução para permitir a execução de scripts não assinados. Aqui está como você pode fazer isso:

1) Abrir PowerShell como Administrador: Clique com o botão direito no ícone do PowerShell e selecione "Executar como administrador".

2) Alterar a Política de Execução: Execute o seguinte comando para permitir a execução de scripts não assinados:
``` Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass ```

3) Executar o Script: Agora, execute o seu script novamente:
``` .\Build-tests-coverage.ps1 ```

4) Reverter a Política de Execução (Opcional): Se desejar, você pode fechar o PowerShell ou reverter a política de execução para o estado anterior após a execução do script:
``` Set-ExecutionPolicy -Scope Process -ExecutionPolicy Default ```

