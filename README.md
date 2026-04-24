# 2026-ei-engsof-a5
Projeto de Engenharia de Software

Professor: Abel Dantas

# Elementos do Grupo:
1. Gilberto Parente - gilbertoparente@ipvc.pt
2. Ruben Lima - limaruben@ipvc.pt
3. João Pereira - o.pereira@ipvc.pt
4. João Cruzeiro - jcruzeiro@ipvc.pt (congelou a matrícula)

# Tema do Grupo : A - Plataforma de gestão de talentos IT 

# Canal de comunicação: Discord
123

# Comandos para instalaçáo dos pacotes entity framework:

1. dotnet add package Microsoft.EntityFrameworkCore --version 7.0.20
2. dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.20
3. dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.20
4. dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 7.0.11
5. dotnet add package BCrypt.Net-Next


## Instruções de Configuração e Execução pela base de dados
1. Script de criação com um backup com dados neste projeto na pasta Database chamado backup_com_dados.sql
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

---------------------------------------------------------------------------
# Este projeto foi desenvolvido utilizando uma metodologia SCRUM

# Sprint 4 
De 13 a 26 de Abril
1. SM - Gilberto Parente


# Sprint Planing
Neste Sprint vamos ter como principais prioridades:
1. Resolução dos Bugs encontrados no Sprint 3:
2. Desenvolvimento de funcionalidades do sprint 4
3. Iniciar testes unitários ao projeto. 
4. Decidir quem será o próximo SM 

# Backlog Sprint 4:
1. Bug 1 - Apresentar as skills da proposta na edição da proposta -
2. Bug 2 - Colocar Data Picker em vez de input manual do ano ao adicionar experiência  
3. Bug 3 - Correção de Salvar experiência  
4. Bug 4 - Ao adicionar experiência, corrigir o gravar a data atual  
5. Bug 5 - Falta ordenar as propostas elegíveis por valor total 
6. 24 - Definir experiência mínima na proposta 
7. 25 - Ordenar resultados da pesquisa 
8. 26 - Gestão de categorias de trabalho 
9. 27 - Criar relatório por categoria e país 
10. Testes unitários
11. Relatorio 
12. Vidoe 
13. Limpeza de repositório
 

# Tarefas a desenvolver no próximo sprint 5
1. Criar relatório por skill
2. Atualizar a documentação - relatório e project
3. Fazer testes funcionais à aplicação
4. Terminar os  testes unitários
5. No decorrer do sprint 5 cada elemento do grupo tem que implementar um design parttern e fazer um video com a explicação e execução.
6. Desenvolver design patterns
7. Desenvolver mecanismos de segurança de navegação e autenticação
8. Decisão de estratégia da alocação da base de dados online
9. Fazer um video completo mostrando todas as funcionalidades

# Proximo SM
Foi decidido que para o Sprint 5 o SM seria o João Pereira

# Retrospetiva
1. Melhor organização do grupo e das divisões das tarefas a desenvolver
2. Dificuldade no desenvolviemnto dos testes unitários
3. dificuldades na atribuição dos design partterns.
4. Foram feitos os testes funcionais por toda a aplicação de modo a validar os inputs e a detetar erros na aplicação
5. Para o iniciar os testes unitário, e com a necessidade de ser criado outro projeto, verificou-se que o github estava a apontar para a pasta do projeto MVC e não para a solução, foi feita a alteração do repositório para a pasta da solução, o que criou algumas dificuldades pois não criamos o .gitignore no inicio, o que levou a enviarmos para o git as pastas "obj" e "bin".



___________________________________________________________


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









