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
