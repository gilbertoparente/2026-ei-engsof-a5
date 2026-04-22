--
-- PostgreSQL database dump
--

-- Dumped from database version 15.2
-- Dumped by pg_dump version 15.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: estadoproposta; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.estadoproposta AS ENUM (
    'ABERTA',
    'EM_PROGRESSO',
    'FECHADA'
);


ALTER TYPE public.estadoproposta OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: areaprofissional; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.areaprofissional (
    idarea integer NOT NULL,
    nome character varying(50) NOT NULL
);


ALTER TABLE public.areaprofissional OWNER TO postgres;

--
-- Name: areaprofissional_idarea_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.areaprofissional_idarea_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.areaprofissional_idarea_seq OWNER TO postgres;

--
-- Name: areaprofissional_idarea_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.areaprofissional_idarea_seq OWNED BY public.areaprofissional.idarea;


--
-- Name: categoriatalento; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.categoriatalento (
    idcategoria integer NOT NULL,
    nome character varying(100) NOT NULL
);


ALTER TABLE public.categoriatalento OWNER TO postgres;

--
-- Name: categoriatalento_idcategoria_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.categoriatalento_idcategoria_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.categoriatalento_idcategoria_seq OWNER TO postgres;

--
-- Name: categoriatalento_idcategoria_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.categoriatalento_idcategoria_seq OWNED BY public.categoriatalento.idcategoria;


--
-- Name: cliente; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.cliente (
    idcliente integer NOT NULL,
    idutilizador integer NOT NULL,
    nome character varying(150) NOT NULL,
    pais character varying(100),
    created_at timestamp without time zone DEFAULT now(),
    updated_at timestamp without time zone DEFAULT now()
);


ALTER TABLE public.cliente OWNER TO postgres;

--
-- Name: cliente_idcliente_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.cliente_idcliente_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.cliente_idcliente_seq OWNER TO postgres;

--
-- Name: cliente_idcliente_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.cliente_idcliente_seq OWNED BY public.cliente.idcliente;


--
-- Name: experiencia; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.experiencia (
    idexperiencia integer NOT NULL,
    idtalento integer NOT NULL,
    empresa character varying(150) NOT NULL,
    anoinicio integer NOT NULL,
    anofim integer,
    descricao text,
    created_at timestamp without time zone DEFAULT now(),
    updated_at timestamp without time zone DEFAULT now()
);


ALTER TABLE public.experiencia OWNER TO postgres;

--
-- Name: experiencia_idexperiencia_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.experiencia_idexperiencia_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.experiencia_idexperiencia_seq OWNER TO postgres;

--
-- Name: experiencia_idexperiencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.experiencia_idexperiencia_seq OWNED BY public.experiencia.idexperiencia;


--
-- Name: propostaskill; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.propostaskill (
    idproposta integer NOT NULL,
    idskill integer NOT NULL,
    anosminimosexperiencia integer,
    CONSTRAINT propostaskill_anosminimosexperiencia_check CHECK ((anosminimosexperiencia >= 0))
);


ALTER TABLE public.propostaskill OWNER TO postgres;

--
-- Name: propostatalento; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.propostatalento (
    id integer NOT NULL,
    idproposta integer NOT NULL,
    idtalento integer NOT NULL,
    datainicio date NOT NULL,
    datafim date,
    estado character varying(20) DEFAULT 'CANDIDATO'::character varying NOT NULL
);


ALTER TABLE public.propostatalento OWNER TO postgres;

--
-- Name: propostatalento_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.propostatalento_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.propostatalento_id_seq OWNER TO postgres;

--
-- Name: propostatalento_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.propostatalento_id_seq OWNED BY public.propostatalento.id;


--
-- Name: propostatrabalho; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.propostatrabalho (
    idproposta integer NOT NULL,
    idutilizador integer NOT NULL,
    idcliente integer NOT NULL,
    nome character varying(200) NOT NULL,
    idcategoria integer NOT NULL,
    horastotais integer,
    descricao text,
    estado character varying(30) DEFAULT 'ABERTA'::public.estadoproposta,
    created_at timestamp without time zone DEFAULT now(),
    updated_at timestamp without time zone DEFAULT now()
);


ALTER TABLE public.propostatrabalho OWNER TO postgres;

--
-- Name: propostatrabalho_idproposta_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.propostatrabalho_idproposta_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.propostatrabalho_idproposta_seq OWNER TO postgres;

--
-- Name: propostatrabalho_idproposta_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.propostatrabalho_idproposta_seq OWNED BY public.propostatrabalho.idproposta;


--
-- Name: skill; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.skill (
    idskill integer NOT NULL,
    nome character varying(100) NOT NULL,
    idarea integer
);


ALTER TABLE public.skill OWNER TO postgres;

--
-- Name: skill_idskill_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.skill_idskill_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.skill_idskill_seq OWNER TO postgres;

--
-- Name: skill_idskill_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.skill_idskill_seq OWNED BY public.skill.idskill;


--
-- Name: talento; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.talento (
    idtalento integer NOT NULL,
    idutilizador integer NOT NULL,
    idcategoria integer NOT NULL,
    pais character varying(100),
    precohora numeric(10,2) NOT NULL,
    publico boolean DEFAULT true,
    created_at timestamp without time zone DEFAULT now(),
    updated_at timestamp without time zone DEFAULT now(),
    nome character varying(150),
    email character varying(150),
    CONSTRAINT talento_precohora_check CHECK ((precohora > (0)::numeric))
);


ALTER TABLE public.talento OWNER TO postgres;

--
-- Name: talento_idtalento_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.talento_idtalento_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.talento_idtalento_seq OWNER TO postgres;

--
-- Name: talento_idtalento_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.talento_idtalento_seq OWNED BY public.talento.idtalento;


--
-- Name: talentoskill; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.talentoskill (
    idtalento integer NOT NULL,
    idskill integer NOT NULL,
    anosexperiencia integer,
    CONSTRAINT talentoskill_anosexperiencia_check CHECK ((anosexperiencia >= 0))
);


ALTER TABLE public.talentoskill OWNER TO postgres;

--
-- Name: utilizador; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.utilizador (
    idutilizador integer NOT NULL,
    nome character varying(150) NOT NULL,
    email character varying(150) NOT NULL,
    passwordhash text NOT NULL,
    created_at timestamp without time zone DEFAULT now(),
    updated_at timestamp without time zone DEFAULT now()
);


ALTER TABLE public.utilizador OWNER TO postgres;

--
-- Name: utilizador_idutilizador_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.utilizador_idutilizador_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.utilizador_idutilizador_seq OWNER TO postgres;

--
-- Name: utilizador_idutilizador_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.utilizador_idutilizador_seq OWNED BY public.utilizador.idutilizador;


--
-- Name: areaprofissional idarea; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.areaprofissional ALTER COLUMN idarea SET DEFAULT nextval('public.areaprofissional_idarea_seq'::regclass);


--
-- Name: categoriatalento idcategoria; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categoriatalento ALTER COLUMN idcategoria SET DEFAULT nextval('public.categoriatalento_idcategoria_seq'::regclass);


--
-- Name: cliente idcliente; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cliente ALTER COLUMN idcliente SET DEFAULT nextval('public.cliente_idcliente_seq'::regclass);


--
-- Name: experiencia idexperiencia; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.experiencia ALTER COLUMN idexperiencia SET DEFAULT nextval('public.experiencia_idexperiencia_seq'::regclass);


--
-- Name: propostatalento id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostatalento ALTER COLUMN id SET DEFAULT nextval('public.propostatalento_id_seq'::regclass);


--
-- Name: propostatrabalho idproposta; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostatrabalho ALTER COLUMN idproposta SET DEFAULT nextval('public.propostatrabalho_idproposta_seq'::regclass);


--
-- Name: skill idskill; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.skill ALTER COLUMN idskill SET DEFAULT nextval('public.skill_idskill_seq'::regclass);


--
-- Name: talento idtalento; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.talento ALTER COLUMN idtalento SET DEFAULT nextval('public.talento_idtalento_seq'::regclass);


--
-- Name: utilizador idutilizador; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.utilizador ALTER COLUMN idutilizador SET DEFAULT nextval('public.utilizador_idutilizador_seq'::regclass);


--
-- Data for Name: areaprofissional; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.areaprofissional (idarea, nome) FROM stdin;
1	Desenvolvimento
2	Design
3	Gestão de Produto
4	Gestão de Projetos
\.


--
-- Data for Name: categoriatalento; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.categoriatalento (idcategoria, nome) FROM stdin;
1	Developer
2	Designer
3	Product Manager
4	Project Manager
\.


--
-- Data for Name: cliente; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.cliente (idcliente, idutilizador, nome, pais, created_at, updated_at) FROM stdin;
1	1	TechCorp	Portugal	2026-03-10 11:45:28.539217	2026-03-10 11:45:28.539217
5	7	TesteCliente	Portugal	2026-04-09 12:53:56.240616	2026-04-09 12:53:56.240802
6	7	Teste 2 cliente	França	2026-04-09 12:54:16.695578	2026-04-09 12:54:16.695579
7	7	ESTG	Portugal	2026-04-13 10:01:53.799262	2026-04-13 10:01:53.799434
4	7	Innovatech	Brasil	2026-03-10 11:45:28.539217	2026-03-10 11:45:28.539217
3	7	DataSolutions	Portugal	2026-03-10 11:45:28.539217	2026-03-10 11:45:28.539217
2	7	DesignHub	Espanha	2026-03-10 11:45:28.539217	2026-03-10 11:45:28.539217
\.


--
-- Data for Name: experiencia; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.experiencia (idexperiencia, idtalento, empresa, anoinicio, anofim, descricao, created_at, updated_at) FROM stdin;
1	1	TechCorp	2020	2022	Desenvolvimento de aplicações web em C# e React	2026-03-10 11:45:56.809548	2026-03-10 11:45:56.809548
2	1	DataSolutions	2022	\N	Projeto atual em Node.js e React	2026-03-10 11:45:56.809548	2026-03-10 11:45:56.809548
3	2	DesignHub	2019	2021	UI/UX Designer em múltiplos projetos	2026-03-10 11:45:56.809548	2026-03-10 11:45:56.809548
4	3	Innovatech	2021	\N	Gestão de produto e coordenação de equipe	2026-03-10 11:45:56.809548	2026-03-10 11:45:56.809548
5	5	ESTG	2020	2021	Teste de software	2026-03-24 11:26:40.245646	2026-03-24 11:26:40.24613
6	6	ESTG	2024	2025	Gestão de projetos	2026-03-24 11:28:54.375061	2026-03-24 11:28:54.375092
7	7	ESTG	2024	2025	levantamento de funcionalidades	2026-03-24 16:26:37.435353	2026-03-24 16:26:37.436052
8	9	ESTG	2025	\N	Trabalhos 	2026-03-29 22:05:09.618052	2026-03-29 22:05:09.618603
\.


--
-- Data for Name: propostaskill; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.propostaskill (idproposta, idskill, anosminimosexperiencia) FROM stdin;
1	1	2
1	2	3
3	5	2
3	3	2
5	2	1
6	7	0
6	8	0
\.


--
-- Data for Name: propostatalento; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.propostatalento (id, idproposta, idtalento, datainicio, datafim, estado) FROM stdin;
6	5	1	2026-04-13	\N	Entrevistado
4	5	7	2026-04-13	\N	Aprovado
\.


--
-- Data for Name: propostatrabalho; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.propostatrabalho (idproposta, idutilizador, idcliente, nome, idcategoria, horastotais, descricao, estado, created_at, updated_at) FROM stdin;
1	1	1	Desenvolvimento Web App	1	120	Projeto de aplicação web em React e C#	ABERTA	2026-03-10 11:46:20.485265	2026-03-10 11:46:20.485265
3	3	4	Gestão de Produto	3	160	Gerenciar backlog e roadmap	EM_PROGRESSO	2026-03-10 11:46:20.485265	2026-03-10 11:46:20.485265
6	1	7	ESTG WEB site	1	20	web site 	ABERTA	2026-04-13 10:19:40.341422	2026-04-13 10:19:40.34185
5	1	5	Teste111	2	111	1111	EM CURSO	\N	2026-04-13 12:16:08.77856
\.


--
-- Data for Name: skill; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.skill (idskill, nome, idarea) FROM stdin;
1	React	1
2	C#	1
3	Time Management	4
4	UI/UX Design	2
5	Project Planning	4
6	Figma	2
7	Node.js	1
8	Scrum	4
10	testeuser	2
\.


--
-- Data for Name: talento; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.talento (idtalento, idutilizador, idcategoria, pais, precohora, publico, created_at, updated_at, nome, email) FROM stdin;
5	7	1	POrtugal	50.00	t	2026-03-24 11:13:19.628377	2026-03-24 11:13:19.628655	Talentos Teste	talentos@gmailcom
6	7	4	Espanha	55.00	f	2026-03-24 11:28:07.413651	2026-03-24 11:28:07.413842	LabTec	lab@gmail.com
7	7	1	Portugal	44.00	t	2026-03-24 16:25:32.571667	2026-03-24 16:25:32.57211	Gilberto	gilberto@gmail.com
1	1	1	Portugal	25.50	t	2026-03-10 11:45:47.075197	2026-03-10 11:45:47.075197	João	email@email.com
2	2	2	Espanha	30.00	t	2026-03-10 11:45:47.075197	2026-03-10 11:45:47.075197	João	email@email.com
3	3	3	Brasil	20.00	f	2026-03-10 11:45:47.075197	2026-03-10 11:45:47.075197	João	email@email.com
4	4	4	Portugal	28.00	t	2026-03-10 11:45:47.075197	2026-03-10 11:45:47.075197	João	email@email.com
8	7	1	Portugal	66.00	t	2026-03-24 16:39:31.023877	2026-03-24 16:39:31.02432	Manuel	manuel@gmail.com
9	7	1	Portugal	45.00	t	2026-03-29 22:04:19.55029	2026-03-29 22:04:19.550456	Gilberto 2	gilberto@gmail.com
\.


--
-- Data for Name: talentoskill; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.talentoskill (idtalento, idskill, anosexperiencia) FROM stdin;
1	1	3
1	2	4
1	7	2
2	4	3
2	6	2
3	3	2
3	5	2
4	3	4
4	8	3
5	2	2
6	8	5
7	2	2
7	5	1
7	1	1
7	8	2
8	8	2
8	1	1
9	1	1
9	2	1
\.


--
-- Data for Name: utilizador; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.utilizador (idutilizador, nome, email, passwordhash, created_at, updated_at) FROM stdin;
1	Alice Silva	alice@exemplo.com	hash123	2026-03-10 11:44:44.531267	2026-03-10 11:44:44.531267
2	Bob Santos	bob@exemplo.com	hash123	2026-03-10 11:44:44.531267	2026-03-10 11:44:44.531267
3	Carla Lima	carla@exemplo.com	hash123	2026-03-10 11:44:44.531267	2026-03-10 11:44:44.531267
4	Daniel Costa	daniel@exemplo.com	hash123	2026-03-10 11:44:44.531267	2026-03-10 11:44:44.531267
5	gil	gil@gmail.com	$2a$11$ZUFIybNNT6riLx9DIu3aku7QmT78CBCrCKQDWHJtmOZ/Kp9y8wmxW	2026-03-16 10:46:59.582129	2026-03-16 10:46:59.58235
6	TesteUser	testeuser@gmail.com	$2a$11$6yHLpteq3ndYZrjHeZ.U2.FQPsEILTzE/Fyl0wIMhXuNUQAWYD28S	2026-03-16 11:25:20.240518	2026-03-16 11:25:20.240782
7	Gilberto	gilberto@gmail.com	$2a$11$qhHwyphTdWalD5mL0psWn.XPKf.YpH.UOQYJoRe3jGpsJIAJz9EoS	2026-03-16 15:19:32.634811	2026-03-16 15:19:32.635011
8	Utilizador1	Utilizador@gmail.com	$2a$11$2Lt4RbcTwYTfSU/Sbw9Educ1LAy1mAit4Xd/7c59Pgf4XIbc2Sf6y	2026-03-16 16:25:00.284174	2026-03-16 16:25:00.284313
\.


--
-- Name: areaprofissional_idarea_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.areaprofissional_idarea_seq', 4, true);


--
-- Name: categoriatalento_idcategoria_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.categoriatalento_idcategoria_seq', 4, true);


--
-- Name: cliente_idcliente_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.cliente_idcliente_seq', 7, true);


--
-- Name: experiencia_idexperiencia_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.experiencia_idexperiencia_seq', 8, true);


--
-- Name: propostatalento_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.propostatalento_id_seq', 6, true);


--
-- Name: propostatrabalho_idproposta_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.propostatrabalho_idproposta_seq', 6, true);


--
-- Name: skill_idskill_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.skill_idskill_seq', 10, true);


--
-- Name: talento_idtalento_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.talento_idtalento_seq', 9, true);


--
-- Name: utilizador_idutilizador_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.utilizador_idutilizador_seq', 8, true);


--
-- Name: areaprofissional areaprofissional_nome_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.areaprofissional
    ADD CONSTRAINT areaprofissional_nome_key UNIQUE (nome);


--
-- Name: areaprofissional areaprofissional_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.areaprofissional
    ADD CONSTRAINT areaprofissional_pkey PRIMARY KEY (idarea);


--
-- Name: categoriatalento categoriatalento_nome_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categoriatalento
    ADD CONSTRAINT categoriatalento_nome_key UNIQUE (nome);


--
-- Name: categoriatalento categoriatalento_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categoriatalento
    ADD CONSTRAINT categoriatalento_pkey PRIMARY KEY (idcategoria);


--
-- Name: cliente cliente_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cliente
    ADD CONSTRAINT cliente_pkey PRIMARY KEY (idcliente);


--
-- Name: experiencia experiencia_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.experiencia
    ADD CONSTRAINT experiencia_pkey PRIMARY KEY (idexperiencia);


--
-- Name: propostaskill propostaskill_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostaskill
    ADD CONSTRAINT propostaskill_pkey PRIMARY KEY (idproposta, idskill);


--
-- Name: propostatalento propostatalento_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostatalento
    ADD CONSTRAINT propostatalento_pkey PRIMARY KEY (id);


--
-- Name: propostatrabalho propostatrabalho_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostatrabalho
    ADD CONSTRAINT propostatrabalho_pkey PRIMARY KEY (idproposta);


--
-- Name: skill skill_nome_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.skill
    ADD CONSTRAINT skill_nome_key UNIQUE (nome);


--
-- Name: skill skill_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.skill
    ADD CONSTRAINT skill_pkey PRIMARY KEY (idskill);


--
-- Name: talento talento_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.talento
    ADD CONSTRAINT talento_pkey PRIMARY KEY (idtalento);


--
-- Name: talentoskill talentoskill_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.talentoskill
    ADD CONSTRAINT talentoskill_pkey PRIMARY KEY (idtalento, idskill);


--
-- Name: experiencia unique_experiencia; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.experiencia
    ADD CONSTRAINT unique_experiencia UNIQUE (idtalento, empresa, anoinicio);


--
-- Name: propostatalento unique_proposta_talento; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostatalento
    ADD CONSTRAINT unique_proposta_talento UNIQUE (idproposta, idtalento);


--
-- Name: utilizador utilizador_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.utilizador
    ADD CONSTRAINT utilizador_email_key UNIQUE (email);


--
-- Name: utilizador utilizador_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.utilizador
    ADD CONSTRAINT utilizador_pkey PRIMARY KEY (idutilizador);


--
-- Name: idx_proposta_categoria; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_proposta_categoria ON public.propostatrabalho USING btree (idcategoria);


--
-- Name: idx_propostaskill_skill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_propostaskill_skill ON public.propostaskill USING btree (idskill);


--
-- Name: idx_talento_categoria; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_talento_categoria ON public.talento USING btree (idcategoria);


--
-- Name: idx_talento_pais; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_talento_pais ON public.talento USING btree (pais);


--
-- Name: idx_talentoskill_skill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_talentoskill_skill ON public.talentoskill USING btree (idskill);


--
-- Name: cliente fk_cliente_utilizador; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cliente
    ADD CONSTRAINT fk_cliente_utilizador FOREIGN KEY (idutilizador) REFERENCES public.utilizador(idutilizador) ON DELETE CASCADE;


--
-- Name: experiencia fk_experiencia_talento; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.experiencia
    ADD CONSTRAINT fk_experiencia_talento FOREIGN KEY (idtalento) REFERENCES public.talento(idtalento) ON DELETE CASCADE;


--
-- Name: propostatalento fk_proposta; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostatalento
    ADD CONSTRAINT fk_proposta FOREIGN KEY (idproposta) REFERENCES public.propostatrabalho(idproposta) ON DELETE CASCADE;


--
-- Name: propostatrabalho fk_proposta_categoria; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostatrabalho
    ADD CONSTRAINT fk_proposta_categoria FOREIGN KEY (idcategoria) REFERENCES public.categoriatalento(idcategoria);


--
-- Name: propostatrabalho fk_proposta_cliente; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostatrabalho
    ADD CONSTRAINT fk_proposta_cliente FOREIGN KEY (idcliente) REFERENCES public.cliente(idcliente);


--
-- Name: propostatrabalho fk_proposta_utilizador; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostatrabalho
    ADD CONSTRAINT fk_proposta_utilizador FOREIGN KEY (idutilizador) REFERENCES public.utilizador(idutilizador);


--
-- Name: propostaskill fk_propostaskill_proposta; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostaskill
    ADD CONSTRAINT fk_propostaskill_proposta FOREIGN KEY (idproposta) REFERENCES public.propostatrabalho(idproposta) ON DELETE CASCADE;


--
-- Name: propostaskill fk_propostaskill_skill; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostaskill
    ADD CONSTRAINT fk_propostaskill_skill FOREIGN KEY (idskill) REFERENCES public.skill(idskill) ON DELETE CASCADE;


--
-- Name: propostatalento fk_talento; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.propostatalento
    ADD CONSTRAINT fk_talento FOREIGN KEY (idtalento) REFERENCES public.talento(idtalento) ON DELETE CASCADE;


--
-- Name: talento fk_talento_categoria; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.talento
    ADD CONSTRAINT fk_talento_categoria FOREIGN KEY (idcategoria) REFERENCES public.categoriatalento(idcategoria);


--
-- Name: talento fk_talento_utilizador; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.talento
    ADD CONSTRAINT fk_talento_utilizador FOREIGN KEY (idutilizador) REFERENCES public.utilizador(idutilizador) ON DELETE CASCADE;


--
-- Name: talentoskill fk_talentoskill_skill; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.talentoskill
    ADD CONSTRAINT fk_talentoskill_skill FOREIGN KEY (idskill) REFERENCES public.skill(idskill) ON DELETE CASCADE;


--
-- Name: talentoskill fk_talentoskill_talento; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.talentoskill
    ADD CONSTRAINT fk_talentoskill_talento FOREIGN KEY (idtalento) REFERENCES public.talento(idtalento) ON DELETE CASCADE;


--
-- Name: skill skill_idarea_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.skill
    ADD CONSTRAINT skill_idarea_fkey FOREIGN KEY (idarea) REFERENCES public.areaprofissional(idarea);


--
-- PostgreSQL database dump complete
--

