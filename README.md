# 2026-ei-engsof-a5
Projeto de Engenharia de Software

Professor: Abel Dantas

# Elementos do Grupo:
1. Gilberto Parente - gilbertoparente@ipvc.pt
2. Ruben LIma - limaruben@ipvc.pt
3. João Pereira - o.pereira@ipvc.pt
4. João Cruzeiro - jcruzeiro@ipvc.pt

# Tema do Grupo : A - Plataforma de gestão de talentos IT 

# Canal de comunicação
Discord link : 

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
•	Desenvolvimento de funcionalidades

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
2. Inserir Dados de teste
3. Efetuar a ligação à base de dados
4. Criar a camada BLL com o comando publicado no ficheiro scratch.txt

# Database connections:
1. jdbc:postgresql://localhost:5432/ProjectManager 

# Comandos para instalaçáo dos pacotes entity framework:

1. dotnet add package Microsoft.EntityFrameworkCore --version 7.0.20
2. dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.20
3. dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.20
4. dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 7.0.11

# Criar as entidades no IDE

1. dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=ProjectManager;Username=postgres;Password=123456" Npgsql.EntityFrameworkCore.PostgreSQL -o ModelsAnager\ProfileMAnager>

# criação da Base de dados :
1. pg_dump -U postgres -d ProjectManager -f database.sql




