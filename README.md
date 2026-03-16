# 2026-ei-engsof-a5
Projeto de Engenharia de Software

Professor: Abel Dantas

# Elementos do Grupo:
1. Gilberto Parente - gilbertoparente@ipvc.pt
2. Ruben LIma - limaruben@ipvc.pt
3. João Pereira - o.pereira@ipvc.pt
4. João Cruzeiro - jcruzeiro@ipvc.pt

# Tema do Grupo : A - Plataforma de gestão de talentos IT 

# Canal de comunicação: Discord

# Tarefas a realizar (05/03/2026)

1. Repositorio
2. Contatos / canal comunicação (disord)
3. Designar Sprints / Scrum Master e colocar no git
4. Fazer backlog

# SPRINT 1
Objetivos do Sprint 1
•	Definir das tarefas
•	Criar backlog do sprint
•	Configuração do desenvolvimento
•	Criação do projeto
•	Desenvolvimento do login/registo

# Backlog Sprint 1

1. Instalar .NET SDK e IDE Rider	Instalar e configurar o ambiente de desenvolvimento	
2. Criar a base de dados em PostgreSQL
3. Criar aplicação Web	Criar projeto Web Application MVC ou Web API + Web App	
4. Configurar ligação à BD	Configurar ligação à base de dados PostgreSQL	
5. Criar camada de dados	Criar camada Model para acesso à base de dados
6. Gerar modelos a partir da BD	Usar scaffolding do Entity Framework para gerar entidades
7. Funcionalidade 1	Registo de utilizadores
8. Funcionalidade 2	Login/Logout	

# Estado da aplicação
1. Base de dados criada
2. Entidades criadas

# Inicialização
1. Correr o Script que está na pasta Database para criar a base de dados no servidor Postgre
2. Inserir Dados de teste, script na pasta Database
3. Efetuar a ligação à base de dados
# Database connections:
1. jdbc:postgresql://localhost:5432/ProjectManager 

# Comandos para instalaçáo dos pacotes entity framework:

1. dotnet add package Microsoft.EntityFrameworkCore --version 7.0.20
2. dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.20
3. dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.20
4. dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 7.0.11
5. dotnet add package BCrypt.Net-Next

# Comando para Criar as entidades no IDE se iniciar pela base de dados
1. dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=ProjectManager;Username=postgres;Password=123456" Npgsql.EntityFrameworkCore.PostgreSQL -o ModelsAnager\ProfileMAnager>

## Instruções de Configuração


### 2. Configurar a Base de Dados
No ficheiro `appsettings.json`, ajuste a `DefaultConnection` com as suas credenciais locais do PostgreSQL:

# json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ProjectManager;Username=postgres;Password=SENHA"
}

3. Criar o Schema e Tabelas
Abra o terminal na pasta raiz do projeto e execute os seguintes comandos do Entity Framework:

# Aplicar as migrações para criar as tabelas no PostgreSQL
dotnet ef database update

4. Carregar Dados de Teste
Script na pasta Database com instruções "insert" para povoar a base de dados para teste





