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

# Comandos para instalaçáo dos pacotes entity framework:

1. dotnet add package Microsoft.EntityFrameworkCore --version 7.0.20
2. dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.20
3. dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.20
4. dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 7.0.11
5. dotnet add package BCrypt.Net-Next


## Instruções de Configuração e Execução pela base de dados
1. Script de criação com um backup com dados neste projeto na pasta Database chamado backup_com_dados
2. No postgres abrir a consola de queries e correr o script


## Instruções de Configuração e Execução pelas entidades

### 1. Preparação da Base de Dados
1. Certifique-se de que o **PostgreSQL** está em execução.
2. Crie uma base de dados vazia chamada `ProjectManager`.
3. No ficheiro `appsettings.json`, configure a sua ligação:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=ProjectManager;Username=postgres;Password=password"
   }
2. Inicialização do Projeto
   Abra o terminal na pasta raiz e execute:

# Instalar dependências e restaurar pacotes
dotnet restore

# Aplicar as migrações (Cria as tabelas automaticamente)
dotnet ef database update

# (Opcional) Se usar scripts para povoar a base de dados de teste:
# Correr o script de insert na pasta Database/DadosTeste.sql
3. Execução
   dotnet run
 

### Sprint 3
Alterações na base de dados:

Criação das tabelas:
1. areaprofissional
2. propostatalento
3. uniformazação dos campos


# Backlog sprint 3 
17 - Criar cliente
18 - Editar cliente
19 - Criar proposta de trabalho
20 - Editar proposta de trabalho
21 - Remover proposta de trabalho
22 - Associar skills a proposta
23 - Definir experiência mínima na proposta


# Estado da aplicação
Propostas funcionais, faltam filtros e melhorias na visualização das propostas. Utilizador associa talentos às propostas no "Editar propostas"
Necessário melhorar logica das propostas



# Backlog Sprint 2:
9 - Eliminar skills
10 - Criar perfil de talento H
11 - Editar perfil de talento
12 - Definir visibilidade do talento
13 - Associar skills a talentos
14 - Definir anos de experiência na skill
15 -Adicionar experiência profissional
16 - Validar sobreposição de experiências


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
3. Tabelas mapeadas
4. Registo criado
5. Login criado
6. tamaticas em desenvolvimento, mas está localmente para já









