
INSERT INTO Utilizador (nome, email, passwordHash)
VALUES
    ('Alice Silva', 'alice@exemplo.com', 'hash123'),
    ('Bob Santos', 'bob@exemplo.com', 'hash123'),
    ('Carla Lima', 'carla@exemplo.com', 'hash123'),
    ('Daniel Costa', 'daniel@exemplo.com', 'hash123');

INSERT INTO AreaProfissional (nome)
VALUES
    ('Desenvolvimento'), ('Design'), ('Gestão de Produto'), ('Gestão de Projetos');

INSERT INTO Skill (nome, idArea)
VALUES
    ('React', 1), ('C#', 1), ('Time Management', 4), ('UI/UX Design', 2),
    ('Project Planning', 4), ('Figma', 2), ('Node.js', 1), ('Scrum', 4);

INSERT INTO Cliente (idUtilizador, nome, pais)
VALUES
    (1, 'TechCorp', 'Portugal'),
    (2, 'DesignHub', 'Espanha'),
    (1, 'DataSolutions', 'Portugal'),
    (3, 'Innovatech', 'Brasil');

INSERT INTO CategoriaTalento (nome)
VALUES ('Developer'), ('Designer'), ('Product Manager'), ('Project Manager');

INSERT INTO Talento (idUtilizador, idCategoria, pais, precoHora, publico)
VALUES
    (1, 1, 'Portugal', 25.50, TRUE),
    (2, 2, 'Espanha', 30.00, TRUE),
    (3, 3, 'Brasil', 20.00, FALSE),
    (4, 4, 'Portugal', 28.00, TRUE);

INSERT INTO Experiencia (idTalento, empresa, anoInicio, anoFim, descricao)
VALUES
    (1, 'TechCorp', 2020, 2022, 'Desenvolvimento de aplicações web em C# e React'),
    (1, 'DataSolutions', 2022, NULL, 'Projeto atual em Node.js e React'),
    (2, 'DesignHub', 2019, 2021, 'UI/UX Designer em múltiplos projetos'),
    (3, 'Innovatech', 2021, NULL, 'Gestão de produto e coordenação de equipe');

INSERT INTO TalentoSkill (idTalento, idSkill, anosExperiencia)
VALUES
    (1, 1, 3), (1, 2, 4), (1, 7, 2),
    (2, 4, 3), (2, 6, 2),
    (3, 3, 2), (3, 5, 2),
    (4, 3, 4), (4, 8, 3);

INSERT INTO PropostaTrabalho (idUtilizador, idCliente, nome, idCategoria, horasTotais, descricao, estado)
VALUES
    (1, 1, 'Desenvolvimento Web App', 1, 120, 'Projeto de aplicação web em React e C#', 'ABERTA'),
    (2, 2, 'Redesign de Aplicativo', 2, 80, 'Novo design UI/UX para aplicativo mobile', 'ABERTA'),
    (3, 4, 'Gestão de Produto', 3, 160, 'Gerenciar backlog e roadmap', 'EM_PROGRESSO');

INSERT INTO PropostaSkill (idProposta, idSkill, anosMinimosExperiencia)
VALUES
    (1, 1, 2), (1, 2, 3),
    (2, 4, 2), (2, 6, 1),
    (3, 5, 2), (3, 3, 2);