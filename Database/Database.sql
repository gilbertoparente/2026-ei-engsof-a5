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


-- Table: public.categoriatalento

-- DROP TABLE IF EXISTS public.categoriatalento;

CREATE TABLE IF NOT EXISTS public.categoriatalento
(
    idcategoria integer NOT NULL DEFAULT 'nextval('categoriatalento_idcategoria_seq'::regclass)',
    nome character varying(100) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT categoriatalento_pkey PRIMARY KEY (idcategoria),
    CONSTRAINT categoriatalento_nome_key UNIQUE (nome)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.categoriatalento
    OWNER to postgres;


-- Table: public.cliente

-- DROP TABLE IF EXISTS public.cliente;

CREATE TABLE IF NOT EXISTS public.cliente
(
    idcliente integer NOT NULL DEFAULT 'nextval('cliente_idcliente_seq'::regclass)',
    idutilizador integer NOT NULL,
    pais character varying(100) COLLATE pg_catalog."default",
    CONSTRAINT cliente_pkey PRIMARY KEY (idcliente),
    CONSTRAINT cliente_idutilizador_key UNIQUE (idutilizador),
    CONSTRAINT fk_cliente_utilizador FOREIGN KEY (idutilizador)
        REFERENCES public.utilizador (idutilizador) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.cliente
    OWNER to postgres;





-- Table: public.experiencia

-- DROP TABLE IF EXISTS public.experiencia;

CREATE TABLE IF NOT EXISTS public.experiencia
(
    idexperiencia integer NOT NULL DEFAULT 'nextval('experiencia_idexperiencia_seq'::regclass)',
    idtalento integer NOT NULL,
    empresa character varying(150) COLLATE pg_catalog."default" NOT NULL,
    anoinicio integer NOT NULL,
    anofim integer,
    descricao text COLLATE pg_catalog."default",
    CONSTRAINT experiencia_pkey PRIMARY KEY (idexperiencia),
    CONSTRAINT unique_experiencia UNIQUE (idtalento, empresa, anoinicio),
    CONSTRAINT fk_experiencia_talento FOREIGN KEY (idtalento)
        REFERENCES public.talento (idtalento) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.experiencia
    OWNER to postgres;


-- Table: public.propostaskill

-- DROP TABLE IF EXISTS public.propostaskill;

CREATE TABLE IF NOT EXISTS public.propostaskill
(
    idproposta integer NOT NULL,
    idskill integer NOT NULL,
    anosminimosexperiencia integer,
    CONSTRAINT propostaskill_pkey PRIMARY KEY (idproposta, idskill),
    CONSTRAINT fk_propostaskill_proposta FOREIGN KEY (idproposta)
        REFERENCES public.propostatrabalho (idproposta) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT fk_propostaskill_skill FOREIGN KEY (idskill)
        REFERENCES public.skill (idskill) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.propostaskill
    OWNER to postgres;
-- Index: idx_propostaskill_skill

-- DROP INDEX IF EXISTS public.idx_propostaskill_skill;

CREATE INDEX IF NOT EXISTS idx_propostaskill_skill
    ON public.propostaskill USING btree
    (idskill ASC NULLS LAST)
    TABLESPACE pg_default;



-- Table: public.propostatrabalho

-- DROP TABLE IF EXISTS public.propostatrabalho;

CREATE TABLE IF NOT EXISTS public.propostatrabalho
(
    idproposta integer NOT NULL DEFAULT 'nextval('propostatrabalho_idproposta_seq'::regclass)',
    idutilizador integer NOT NULL,
    idcliente integer NOT NULL,
    nome character varying(200) COLLATE pg_catalog."default" NOT NULL,
    idcategoria integer,
    horastotais integer,
    descricao text COLLATE pg_catalog."default",
    estado character varying(20) COLLATE pg_catalog."default" DEFAULT 'ABERTA'::character varying,
    CONSTRAINT propostatrabalho_pkey PRIMARY KEY (idproposta),
    CONSTRAINT fk_proposta_categoria FOREIGN KEY (idcategoria)
        REFERENCES public.categoriatalento (idcategoria) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_proposta_cliente FOREIGN KEY (idcliente)
        REFERENCES public.cliente (idcliente) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_proposta_utilizador FOREIGN KEY (idutilizador)
        REFERENCES public.utilizador (idutilizador) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.propostatrabalho
    OWNER to postgres;




-- Table: public.skill

-- DROP TABLE IF EXISTS public.skill;

CREATE TABLE IF NOT EXISTS public.skill
(
    idskill integer NOT NULL DEFAULT 'nextval('skill_idskill_seq'::regclass)',
    nome character varying(100) COLLATE pg_catalog."default" NOT NULL,
    areaprofissional character varying(100) COLLATE pg_catalog."default",
    CONSTRAINT skill_pkey PRIMARY KEY (idskill),
    CONSTRAINT skill_nome_key UNIQUE (nome)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.skill
    OWNER to postgres;




-- Table: public.talento

-- DROP TABLE IF EXISTS public.talento;

CREATE TABLE IF NOT EXISTS public.talento
(
    idtalento integer NOT NULL DEFAULT 'nextval('talento_idtalento_seq'::regclass)',
    idutilizador integer NOT NULL,
    idcategoria integer,
    pais character varying(100) COLLATE pg_catalog."default",
    precohora numeric(10,2) NOT NULL,
    publico boolean DEFAULT 'true',
    nome character varying(150) COLLATE pg_catalog."default",
    email character varying(150) COLLATE pg_catalog."default",
    CONSTRAINT talento_pkey PRIMARY KEY (idtalento),
    CONSTRAINT talento_idutilizador_key UNIQUE (idutilizador),
    CONSTRAINT fk_talento_categoria FOREIGN KEY (idcategoria)
        REFERENCES public.categoriatalento (idcategoria) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_talento_utilizador FOREIGN KEY (idutilizador)
        REFERENCES public.utilizador (idutilizador) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT check_preco CHECK (precohora > 0::numeric)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.talento
    OWNER to postgres;



-- Table: public.talentoskill

-- DROP TABLE IF EXISTS public.talentoskill;

CREATE TABLE IF NOT EXISTS public.talentoskill
(
    idtalento integer NOT NULL,
    idskill integer NOT NULL,
    anosexperiencia integer,
    CONSTRAINT talentoskill_pkey PRIMARY KEY (idtalento, idskill),
    CONSTRAINT fk_talentoskill_skill FOREIGN KEY (idskill)
        REFERENCES public.skill (idskill) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT fk_talentoskill_talento FOREIGN KEY (idtalento)
        REFERENCES public.talento (idtalento) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.talentoskill
    OWNER to postgres;
-- Index: idx_talentoskill_skill

-- DROP INDEX IF EXISTS public.idx_talentoskill_skill;

CREATE INDEX IF NOT EXISTS idx_talentoskill_skill
    ON public.talentoskill USING btree
    (idskill ASC NULLS LAST)
    TABLESPACE pg_default;




-- Table: public.utilizador

-- DROP TABLE IF EXISTS public.utilizador;

CREATE TABLE IF NOT EXISTS public.utilizador
(
    idutilizador integer NOT NULL DEFAULT 'nextval('utilizador_idutilizador_seq'::regclass)',
    nome character varying(150) COLLATE pg_catalog."default" NOT NULL,
    email character varying(150) COLLATE pg_catalog."default" NOT NULL,
    passwordhash text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT utilizador_pkey PRIMARY KEY (idutilizador),
    CONSTRAINT utilizador_email_key UNIQUE (email)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.utilizador
    OWNER to postgres;








