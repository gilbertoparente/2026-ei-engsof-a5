-- Database: plataformaTalentos

-- DROP DATABASE IF EXISTS "plataformaTalentos";

CREATE DATABASE "plataformaTalentos"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Portuguese_Brazil.1252'
    LC_CTYPE = 'Portuguese_Brazil.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;



-- TABELA UTILIZADOR
CREATE TABLE Utilizador (
                            idUtilizador SERIAL PRIMARY KEY,
                            nome VARCHAR(150) NOT NULL,
                            email VARCHAR(150) UNIQUE NOT NULL,
                            passwordHash TEXT NOT NULL,
                            created_at TIMESTAMP DEFAULT NOW(),
                            updated_at TIMESTAMP DEFAULT NOW()
);

-- TABELA AREA PROFISSIONAL
CREATE TABLE AreaProfissional (
                                  idArea SERIAL PRIMARY KEY,
                                  nome VARCHAR(50) UNIQUE NOT NULL
);

-- TABELA SKILL
CREATE TABLE Skill (
                       idSkill SERIAL PRIMARY KEY,
                       nome VARCHAR(100) UNIQUE NOT NULL,
                       idArea INT REFERENCES AreaProfissional(idArea)
);

-- TABELA CLIENTE
CREATE TABLE Cliente (
                         idCliente SERIAL PRIMARY KEY,
                         idUtilizador INT NOT NULL,
                         nome VARCHAR(150) NOT NULL,
                         pais VARCHAR(100),
                         created_at TIMESTAMP DEFAULT NOW(),
                         updated_at TIMESTAMP DEFAULT NOW(),
                         CONSTRAINT fk_cliente_utilizador
                             FOREIGN KEY (idUtilizador)
                                 REFERENCES Utilizador(idUtilizador)
                                 ON DELETE CASCADE
);

-- TABELA CATEGORIA TALENTO
CREATE TABLE CategoriaTalento (
                                  idCategoria SERIAL PRIMARY KEY,
                                  nome VARCHAR(100) UNIQUE NOT NULL
);

-- TABELA TALENTO
CREATE TABLE Talento (
                         idTalento SERIAL PRIMARY KEY,
                         idUtilizador INT NOT NULL,
                         idCategoria INT NOT NULL,
                         pais VARCHAR(100),
                         precoHora DECIMAL(10,2) NOT NULL CHECK (precoHora > 0),
                         publico BOOLEAN DEFAULT TRUE,
                         created_at TIMESTAMP DEFAULT NOW(),
                         updated_at TIMESTAMP DEFAULT NOW(),
                         CONSTRAINT fk_talento_utilizador
                             FOREIGN KEY (idUtilizador)
                                 REFERENCES Utilizador(idUtilizador)
                                 ON DELETE CASCADE,
                         CONSTRAINT fk_talento_categoria
                             FOREIGN KEY (idCategoria)
                                 REFERENCES CategoriaTalento(idCategoria)
);

-- TABELA EXPERIENCIA
CREATE TABLE Experiencia (
                             idExperiencia SERIAL PRIMARY KEY,
                             idTalento INT NOT NULL,
                             empresa VARCHAR(150) NOT NULL,
                             anoInicio INT NOT NULL,
                             anoFim INT,
                             descricao TEXT,
                             created_at TIMESTAMP DEFAULT NOW(),
                             updated_at TIMESTAMP DEFAULT NOW(),
                             CONSTRAINT fk_experiencia_talento
                                 FOREIGN KEY (idTalento)
                                     REFERENCES Talento(idTalento)
                                     ON DELETE CASCADE,
                             CONSTRAINT unique_experiencia
                                 UNIQUE(idTalento, empresa, anoInicio)
);

-- TABELA TALENTO SKILL (N:N)
CREATE TABLE TalentoSkill (
                              idTalento INT NOT NULL,
                              idSkill INT NOT NULL,
                              anosExperiencia INT CHECK (anosExperiencia >= 0),
                              PRIMARY KEY (idTalento, idSkill),
                              CONSTRAINT fk_talentoSkill_talento
                                  FOREIGN KEY (idTalento)
                                      REFERENCES Talento(idTalento)
                                      ON DELETE CASCADE,
                              CONSTRAINT fk_talentoSkill_skill
                                  FOREIGN KEY (idSkill)
                                      REFERENCES Skill(idSkill)
                                      ON DELETE CASCADE
);

-- ENUM PARA ESTADO DE PROPOSTA
CREATE TYPE EstadoProposta AS ENUM ('ABERTA', 'EM_PROGRESSO', 'FECHADA');

-- TABELA PROPOSTA TRABALHO
CREATE TABLE PropostaTrabalho (
                                  idProposta SERIAL PRIMARY KEY,
                                  idUtilizador INT NOT NULL,
                                  idCliente INT NOT NULL,
                                  nome VARCHAR(200) NOT NULL,
                                  idCategoria INT NOT NULL,
                                  horasTotais INT,
                                  descricao TEXT,
                                  estado EstadoProposta DEFAULT 'ABERTA',
                                  created_at TIMESTAMP DEFAULT NOW(),
                                  updated_at TIMESTAMP DEFAULT NOW(),
                                  CONSTRAINT fk_proposta_utilizador
                                      FOREIGN KEY (idUtilizador)
                                          REFERENCES Utilizador(idUtilizador),
                                  CONSTRAINT fk_proposta_cliente
                                      FOREIGN KEY (idCliente)
                                          REFERENCES Cliente(idCliente),
                                  CONSTRAINT fk_proposta_categoria
                                      FOREIGN KEY (idCategoria)
                                          REFERENCES CategoriaTalento(idCategoria)
);

-- TABELA PROPOSTA SKILL (N:N)
CREATE TABLE PropostaSkill (
                               idProposta INT NOT NULL,
                               idSkill INT NOT NULL,
                               anosMinimosExperiencia INT CHECK (anosMinimosExperiencia >= 0),
                               PRIMARY KEY (idProposta, idSkill),
                               CONSTRAINT fk_propostaSkill_proposta
                                   FOREIGN KEY (idProposta)
                                       REFERENCES PropostaTrabalho(idProposta)
                                       ON DELETE CASCADE,
                               CONSTRAINT fk_propostaSkill_skill
                                   FOREIGN KEY (idSkill)
                                       REFERENCES Skill(idSkill)
                                       ON DELETE CASCADE
);

-- ÍNDICES PARA PERFORMANCE
CREATE INDEX idx_talentoSkill_skill ON TalentoSkill(idSkill);
CREATE INDEX idx_propostaSkill_skill ON PropostaSkill(idSkill);
CREATE INDEX idx_talento_pais ON Talento(pais);
CREATE INDEX idx_talento_categoria ON Talento(idCategoria);
CREATE INDEX idx_proposta_categoria ON PropostaTrabalho(idCategoria);

