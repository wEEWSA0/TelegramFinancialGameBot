--
-- PostgreSQL database cluster dump
--

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Drop databases (except postgres and template1)
--

DROP DATABASE weewsa_financialgamebot_db;




--
-- Drop roles
--

DROP ROLE postgres;


--
-- Roles
--

CREATE ROLE postgres;
ALTER ROLE postgres WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS PASSWORD 'SCRAM-SHA-256$4096:Xr/1fkJr7IW5EnSDqPKE6g==$XBHVVgbtPc9MRX68naSGrkzCy2rK0PllGX62F0Q9YIc=:moIUm0WNxUrhYcn4mG/OgrE4E1nujjIYl8WKBeIlYew=';

--
-- User Configurations
--








--
-- Databases
--

--
-- Database "template1" dump
--

--
-- PostgreSQL database dump
--

-- Dumped from database version 15.3 (Debian 15.3-1.pgdg120+1)
-- Dumped by pg_dump version 15.3 (Debian 15.3-1.pgdg120+1)

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

UPDATE pg_catalog.pg_database SET datistemplate = false WHERE datname = 'template1';
DROP DATABASE template1;
--
-- Name: template1; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE template1 WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE template1 OWNER TO postgres;

\connect template1

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
-- Name: DATABASE template1; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON DATABASE template1 IS 'default template for new databases';


--
-- Name: template1; Type: DATABASE PROPERTIES; Schema: -; Owner: postgres
--

ALTER DATABASE template1 IS_TEMPLATE = true;


\connect template1

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
-- Name: DATABASE template1; Type: ACL; Schema: -; Owner: postgres
--

REVOKE CONNECT,TEMPORARY ON DATABASE template1 FROM PUBLIC;
GRANT CONNECT ON DATABASE template1 TO PUBLIC;


--
-- PostgreSQL database dump complete
--

--
-- Database "postgres" dump
--

--
-- PostgreSQL database dump
--

-- Dumped from database version 15.3 (Debian 15.3-1.pgdg120+1)
-- Dumped by pg_dump version 15.3 (Debian 15.3-1.pgdg120+1)

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

DROP DATABASE postgres;
--
-- Name: postgres; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE postgres WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE postgres OWNER TO postgres;

\connect postgres

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
-- Name: DATABASE postgres; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON DATABASE postgres IS 'default administrative connection database';


--
-- PostgreSQL database dump complete
--

--
-- Database "weewsa_financialgamebot_db" dump
--

--
-- PostgreSQL database dump
--

-- Dumped from database version 15.3 (Debian 15.3-1.pgdg120+1)
-- Dumped by pg_dump version 15.3 (Debian 15.3-1.pgdg120+1)

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
-- Name: weewsa_financialgamebot_db; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE weewsa_financialgamebot_db WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE weewsa_financialgamebot_db OWNER TO postgres;

\connect weewsa_financialgamebot_db

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

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Accidents; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Accidents" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "CashExpense" integer NOT NULL,
    "TimeExpense" smallint NOT NULL,
    "EnergyCost" smallint NOT NULL,
    "StepsDuration" smallint NOT NULL,
    "Cost" integer NOT NULL,
    "Type" integer NOT NULL
);


ALTER TABLE public."Accidents" OWNER TO postgres;

--
-- Name: Accounts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Accounts" (
    "ChatId" bigint NOT NULL,
    "Name" text NOT NULL,
    "isLink" boolean NOT NULL
);


ALTER TABLE public."Accounts" OWNER TO postgres;

--
-- Name: Accounts_ChatId_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Accounts" ALTER COLUMN "ChatId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Accounts_ChatId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: BuisnessManagerStaffs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."BuisnessManagerStaffs" (
    "UserBuisnessId" integer NOT NULL,
    "ManagerStaffId" text NOT NULL
);


ALTER TABLE public."BuisnessManagerStaffs" OWNER TO postgres;

--
-- Name: BuisnessRegionalDirectors; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."BuisnessRegionalDirectors" (
    "UserBuisnessId" integer NOT NULL,
    "RegionalDirectorId" text NOT NULL
);


ALTER TABLE public."BuisnessRegionalDirectors" OWNER TO postgres;

--
-- Name: Buisnesses; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Buisnesses" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "RequireTime" smallint NOT NULL,
    "VariableExpenses" smallint NOT NULL,
    "CashIncome" integer NOT NULL,
    "CashExpense" integer NOT NULL,
    "Cost" integer NOT NULL
);


ALTER TABLE public."Buisnesses" OWNER TO postgres;

--
-- Name: Dreams; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Dreams" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "StepCount" smallint NOT NULL,
    "RequireTime" smallint NOT NULL,
    "CashExpense" integer NOT NULL,
    "Cost" integer NOT NULL
);


ALTER TABLE public."Dreams" OWNER TO postgres;

--
-- Name: FinancialDirectors; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."FinancialDirectors" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "CashIncomePercent" smallint NOT NULL,
    "CashExpense" integer NOT NULL,
    "TimeIncome" smallint NOT NULL
);


ALTER TABLE public."FinancialDirectors" OWNER TO postgres;

--
-- Name: GeneralDirectors; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."GeneralDirectors" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "CashIncomePercent" smallint NOT NULL,
    "CashExpense" integer NOT NULL,
    "TimeIncome" smallint NOT NULL
);


ALTER TABLE public."GeneralDirectors" OWNER TO postgres;

--
-- Name: KnowledgeForWork; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."KnowledgeForWork" (
    "KnowledgeId" text NOT NULL,
    "WorkId" text NOT NULL
);


ALTER TABLE public."KnowledgeForWork" OWNER TO postgres;

--
-- Name: Knowledges; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Knowledges" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "LearningTime" smallint NOT NULL,
    "RequireTime" smallint NOT NULL,
    "CashExpense" integer NOT NULL,
    "Cost" integer NOT NULL
);


ALTER TABLE public."Knowledges" OWNER TO postgres;

--
-- Name: ManagerStaffs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ManagerStaffs" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "CashExpense" integer NOT NULL,
    "TimeIncome" smallint NOT NULL
);


ALTER TABLE public."ManagerStaffs" OWNER TO postgres;

--
-- Name: Properties; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Properties" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "RentCashIncome" integer NOT NULL,
    "TimeExpense" smallint NOT NULL,
    "CashExpense" integer NOT NULL,
    "Cost" integer NOT NULL,
    "EnergyCost" smallint NOT NULL
);


ALTER TABLE public."Properties" OWNER TO postgres;

--
-- Name: RegionalDirectors; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."RegionalDirectors" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "CashExpense" integer NOT NULL,
    "TimeIncome" smallint NOT NULL
);


ALTER TABLE public."RegionalDirectors" OWNER TO postgres;

--
-- Name: Rooms; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Rooms" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "Step" smallint NOT NULL,
    "OwnerChatId" bigint NOT NULL
);


ALTER TABLE public."Rooms" OWNER TO postgres;

--
-- Name: Rooms_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Rooms" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Rooms_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: SetupCharacterCards; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."SetupCharacterCards" (
    "SetupCharacterId" text NOT NULL,
    "Card" text NOT NULL,
    "AdditionalInfo" text
);


ALTER TABLE public."SetupCharacterCards" OWNER TO postgres;

--
-- Name: SetupCharacters; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."SetupCharacters" (
    "Id" text NOT NULL,
    "Cash" integer NOT NULL,
    "FreeTime" integer NOT NULL
);


ALTER TABLE public."SetupCharacters" OWNER TO postgres;

--
-- Name: Staffs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Staffs" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "CashExpense" integer NOT NULL,
    "TimeIncome" smallint NOT NULL
);


ALTER TABLE public."Staffs" OWNER TO postgres;

--
-- Name: UserAccidents; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserAccidents" (
    "UserId" integer NOT NULL,
    "AccidentId" text NOT NULL,
    "CurrentStep" smallint NOT NULL
);


ALTER TABLE public."UserAccidents" OWNER TO postgres;

--
-- Name: UserBuisnesses; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserBuisnesses" (
    "Id" integer NOT NULL,
    "UserId" integer NOT NULL,
    "BuisnessId" text NOT NULL,
    "BranchCount" smallint NOT NULL,
    "OpenSteps" smallint NOT NULL,
    "FinancialDirectorId" text,
    "GeneralDirectorId" text
);


ALTER TABLE public."UserBuisnesses" OWNER TO postgres;

--
-- Name: UserBuisnesses_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."UserBuisnesses" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."UserBuisnesses_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: UserDreamExpectations; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserDreamExpectations" (
    "UserId" integer NOT NULL,
    "DreamId" text NOT NULL,
    "Steps" smallint NOT NULL
);


ALTER TABLE public."UserDreamExpectations" OWNER TO postgres;

--
-- Name: UserKnowledges; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserKnowledges" (
    "UserId" integer NOT NULL,
    "KnowledgeId" text NOT NULL,
    "TimeToLearn" smallint NOT NULL,
    "Experience" smallint NOT NULL
);


ALTER TABLE public."UserKnowledges" OWNER TO postgres;

--
-- Name: UserProperties; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserProperties" (
    "UserId" integer NOT NULL,
    "PropertyId" text NOT NULL,
    "UsesAsHome" boolean NOT NULL,
    "IsOwner" boolean NOT NULL
);


ALTER TABLE public."UserProperties" OWNER TO postgres;

--
-- Name: UserStaffs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserStaffs" (
    "UserId" integer NOT NULL,
    "StaffId" text NOT NULL
);


ALTER TABLE public."UserStaffs" OWNER TO postgres;

--
-- Name: UserWorkPositions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserWorkPositions" (
    "UserId" integer NOT NULL,
    "WorkPositionId" text NOT NULL,
    "WorkPositionWorkId" text NOT NULL,
    "WorkPositionExpirienceRequire" smallint NOT NULL,
    "Experience" smallint NOT NULL
);


ALTER TABLE public."UserWorkPositions" OWNER TO postgres;

--
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" integer NOT NULL,
    "AccountChatId" bigint NOT NULL,
    "RoomId" integer NOT NULL,
    "DreamId" text,
    "CompleteDream" boolean NOT NULL,
    "Cash" integer NOT NULL,
    "FreeTime" integer NOT NULL,
    "CashIncome" integer NOT NULL,
    "Energy" smallint NOT NULL,
    "FinishedStep" boolean NOT NULL
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- Name: Users_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Users" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Users_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: VictoryConditions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."VictoryConditions" (
    "RoomId" integer NOT NULL,
    "CashIncome" integer NOT NULL,
    "RequireTime" smallint NOT NULL,
    "TimeForPaymentsToBank" smallint NOT NULL,
    "ShouldDreamBeCompleted" boolean NOT NULL
);


ALTER TABLE public."VictoryConditions" OWNER TO postgres;

--
-- Name: WorkPositions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."WorkPositions" (
    "WorkId" text NOT NULL,
    "ExpirienceRequire" smallint NOT NULL,
    "Income" integer NOT NULL,
    "RequireTime" smallint NOT NULL
);


ALTER TABLE public."WorkPositions" OWNER TO postgres;

--
-- Name: Works; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Works" (
    "Id" text NOT NULL,
    "Name" text NOT NULL
);


ALTER TABLE public."Works" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Accidents; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Accidents" ("Id", "Name", "CashExpense", "TimeExpense", "EnergyCost", "StepsDuration", "Cost", "Type") FROM stdin;
\.


--
-- Data for Name: Accounts; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Accounts" ("ChatId", "Name", "isLink") FROM stdin;
6178425342	Alex	t
\.


--
-- Data for Name: BuisnessManagerStaffs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."BuisnessManagerStaffs" ("UserBuisnessId", "ManagerStaffId") FROM stdin;
\.


--
-- Data for Name: BuisnessRegionalDirectors; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."BuisnessRegionalDirectors" ("UserBuisnessId", "RegionalDirectorId") FROM stdin;
\.


--
-- Data for Name: Buisnesses; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Buisnesses" ("Id", "Name", "RequireTime", "VariableExpenses", "CashIncome", "CashExpense", "Cost") FROM stdin;
\.


--
-- Data for Name: Dreams; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Dreams" ("Id", "Name", "StepCount", "RequireTime", "CashExpense", "Cost") FROM stdin;
\.


--
-- Data for Name: FinancialDirectors; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."FinancialDirectors" ("Id", "Name", "CashIncomePercent", "CashExpense", "TimeIncome") FROM stdin;
\.


--
-- Data for Name: GeneralDirectors; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."GeneralDirectors" ("Id", "Name", "CashIncomePercent", "CashExpense", "TimeIncome") FROM stdin;
\.


--
-- Data for Name: KnowledgeForWork; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."KnowledgeForWork" ("KnowledgeId", "WorkId") FROM stdin;
\.


--
-- Data for Name: Knowledges; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Knowledges" ("Id", "Name", "LearningTime", "RequireTime", "CashExpense", "Cost") FROM stdin;
\.


--
-- Data for Name: ManagerStaffs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."ManagerStaffs" ("Id", "Name", "CashExpense", "TimeIncome") FROM stdin;
\.


--
-- Data for Name: Properties; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Properties" ("Id", "Name", "RentCashIncome", "TimeExpense", "CashExpense", "Cost", "EnergyCost") FROM stdin;
\.


--
-- Data for Name: RegionalDirectors; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."RegionalDirectors" ("Id", "Name", "CashExpense", "TimeIncome") FROM stdin;
\.


--
-- Data for Name: Rooms; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Rooms" ("Id", "Name", "Step", "OwnerChatId") FROM stdin;
1	test	2	6178425342
\.


--
-- Data for Name: SetupCharacterCards; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."SetupCharacterCards" ("SetupCharacterId", "Card", "AdditionalInfo") FROM stdin;
\.


--
-- Data for Name: SetupCharacters; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."SetupCharacters" ("Id", "Cash", "FreeTime") FROM stdin;
base	1223232	700
\.


--
-- Data for Name: Staffs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Staffs" ("Id", "Name", "CashExpense", "TimeIncome") FROM stdin;
\.


--
-- Data for Name: UserAccidents; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserAccidents" ("UserId", "AccidentId", "CurrentStep") FROM stdin;
\.


--
-- Data for Name: UserBuisnesses; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserBuisnesses" ("Id", "UserId", "BuisnessId", "BranchCount", "OpenSteps", "FinancialDirectorId", "GeneralDirectorId") FROM stdin;
\.


--
-- Data for Name: UserDreamExpectations; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserDreamExpectations" ("UserId", "DreamId", "Steps") FROM stdin;
\.


--
-- Data for Name: UserKnowledges; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserKnowledges" ("UserId", "KnowledgeId", "TimeToLearn", "Experience") FROM stdin;
\.


--
-- Data for Name: UserProperties; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserProperties" ("UserId", "PropertyId", "UsesAsHome", "IsOwner") FROM stdin;
\.


--
-- Data for Name: UserStaffs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserStaffs" ("UserId", "StaffId") FROM stdin;
\.


--
-- Data for Name: UserWorkPositions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserWorkPositions" ("UserId", "WorkPositionId", "WorkPositionWorkId", "WorkPositionExpirienceRequire", "Experience") FROM stdin;
\.


--
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("Id", "AccountChatId", "RoomId", "DreamId", "CompleteDream", "Cash", "FreeTime", "CashIncome", "Energy", "FinishedStep") FROM stdin;
1	6178425342	1	\N	f	1163232	700	0	24	f
\.


--
-- Data for Name: VictoryConditions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."VictoryConditions" ("RoomId", "CashIncome", "RequireTime", "TimeForPaymentsToBank", "ShouldDreamBeCompleted") FROM stdin;
1	1000000	450	6	t
\.


--
-- Data for Name: WorkPositions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."WorkPositions" ("WorkId", "ExpirienceRequire", "Income", "RequireTime") FROM stdin;
\.


--
-- Data for Name: Works; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Works" ("Id", "Name") FROM stdin;
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20230726210406_AddUserDreamExpectation	7.0.9
20230727121037_AddAccident	7.0.9
20230727123426_AddUserAccident	7.0.9
\.


--
-- Name: Accounts_ChatId_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Accounts_ChatId_seq"', 1, false);


--
-- Name: Rooms_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Rooms_Id_seq"', 1, true);


--
-- Name: UserBuisnesses_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."UserBuisnesses_Id_seq"', 1, false);


--
-- Name: Users_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Users_Id_seq"', 1, true);


--
-- Name: Accidents PK_Accidents; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Accidents"
    ADD CONSTRAINT "PK_Accidents" PRIMARY KEY ("Id");


--
-- Name: Accounts PK_Accounts; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Accounts"
    ADD CONSTRAINT "PK_Accounts" PRIMARY KEY ("ChatId");


--
-- Name: BuisnessManagerStaffs PK_BuisnessManagerStaffs; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BuisnessManagerStaffs"
    ADD CONSTRAINT "PK_BuisnessManagerStaffs" PRIMARY KEY ("UserBuisnessId", "ManagerStaffId");


--
-- Name: BuisnessRegionalDirectors PK_BuisnessRegionalDirectors; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BuisnessRegionalDirectors"
    ADD CONSTRAINT "PK_BuisnessRegionalDirectors" PRIMARY KEY ("UserBuisnessId", "RegionalDirectorId");


--
-- Name: Buisnesses PK_Buisnesses; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Buisnesses"
    ADD CONSTRAINT "PK_Buisnesses" PRIMARY KEY ("Id");


--
-- Name: Dreams PK_Dreams; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Dreams"
    ADD CONSTRAINT "PK_Dreams" PRIMARY KEY ("Id");


--
-- Name: FinancialDirectors PK_FinancialDirectors; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FinancialDirectors"
    ADD CONSTRAINT "PK_FinancialDirectors" PRIMARY KEY ("Id");


--
-- Name: GeneralDirectors PK_GeneralDirectors; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."GeneralDirectors"
    ADD CONSTRAINT "PK_GeneralDirectors" PRIMARY KEY ("Id");


--
-- Name: KnowledgeForWork PK_KnowledgeForWork; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."KnowledgeForWork"
    ADD CONSTRAINT "PK_KnowledgeForWork" PRIMARY KEY ("KnowledgeId", "WorkId");


--
-- Name: Knowledges PK_Knowledges; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Knowledges"
    ADD CONSTRAINT "PK_Knowledges" PRIMARY KEY ("Id");


--
-- Name: ManagerStaffs PK_ManagerStaffs; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ManagerStaffs"
    ADD CONSTRAINT "PK_ManagerStaffs" PRIMARY KEY ("Id");


--
-- Name: Properties PK_Properties; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Properties"
    ADD CONSTRAINT "PK_Properties" PRIMARY KEY ("Id");


--
-- Name: RegionalDirectors PK_RegionalDirectors; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RegionalDirectors"
    ADD CONSTRAINT "PK_RegionalDirectors" PRIMARY KEY ("Id");


--
-- Name: Rooms PK_Rooms; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Rooms"
    ADD CONSTRAINT "PK_Rooms" PRIMARY KEY ("Id");


--
-- Name: SetupCharacterCards PK_SetupCharacterCards; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SetupCharacterCards"
    ADD CONSTRAINT "PK_SetupCharacterCards" PRIMARY KEY ("SetupCharacterId", "Card");


--
-- Name: SetupCharacters PK_SetupCharacters; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SetupCharacters"
    ADD CONSTRAINT "PK_SetupCharacters" PRIMARY KEY ("Id");


--
-- Name: Staffs PK_Staffs; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Staffs"
    ADD CONSTRAINT "PK_Staffs" PRIMARY KEY ("Id");


--
-- Name: UserAccidents PK_UserAccidents; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserAccidents"
    ADD CONSTRAINT "PK_UserAccidents" PRIMARY KEY ("UserId", "AccidentId");


--
-- Name: UserBuisnesses PK_UserBuisnesses; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserBuisnesses"
    ADD CONSTRAINT "PK_UserBuisnesses" PRIMARY KEY ("Id");


--
-- Name: UserDreamExpectations PK_UserDreamExpectations; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserDreamExpectations"
    ADD CONSTRAINT "PK_UserDreamExpectations" PRIMARY KEY ("UserId", "DreamId");


--
-- Name: UserKnowledges PK_UserKnowledges; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserKnowledges"
    ADD CONSTRAINT "PK_UserKnowledges" PRIMARY KEY ("UserId", "KnowledgeId");


--
-- Name: UserProperties PK_UserProperties; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserProperties"
    ADD CONSTRAINT "PK_UserProperties" PRIMARY KEY ("UserId", "PropertyId");


--
-- Name: UserStaffs PK_UserStaffs; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserStaffs"
    ADD CONSTRAINT "PK_UserStaffs" PRIMARY KEY ("UserId", "StaffId");


--
-- Name: UserWorkPositions PK_UserWorkPositions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserWorkPositions"
    ADD CONSTRAINT "PK_UserWorkPositions" PRIMARY KEY ("UserId", "WorkPositionId");


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- Name: VictoryConditions PK_VictoryConditions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."VictoryConditions"
    ADD CONSTRAINT "PK_VictoryConditions" PRIMARY KEY ("RoomId");


--
-- Name: WorkPositions PK_WorkPositions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."WorkPositions"
    ADD CONSTRAINT "PK_WorkPositions" PRIMARY KEY ("WorkId", "ExpirienceRequire");


--
-- Name: Works PK_Works; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Works"
    ADD CONSTRAINT "PK_Works" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_BuisnessManagerStaffs_ManagerStaffId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_BuisnessManagerStaffs_ManagerStaffId" ON public."BuisnessManagerStaffs" USING btree ("ManagerStaffId");


--
-- Name: IX_BuisnessRegionalDirectors_RegionalDirectorId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_BuisnessRegionalDirectors_RegionalDirectorId" ON public."BuisnessRegionalDirectors" USING btree ("RegionalDirectorId");


--
-- Name: IX_KnowledgeForWork_WorkId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_KnowledgeForWork_WorkId" ON public."KnowledgeForWork" USING btree ("WorkId");


--
-- Name: IX_UserAccidents_AccidentId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserAccidents_AccidentId" ON public."UserAccidents" USING btree ("AccidentId");


--
-- Name: IX_UserBuisnesses_BuisnessId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserBuisnesses_BuisnessId" ON public."UserBuisnesses" USING btree ("BuisnessId");


--
-- Name: IX_UserBuisnesses_FinancialDirectorId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserBuisnesses_FinancialDirectorId" ON public."UserBuisnesses" USING btree ("FinancialDirectorId");


--
-- Name: IX_UserBuisnesses_GeneralDirectorId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserBuisnesses_GeneralDirectorId" ON public."UserBuisnesses" USING btree ("GeneralDirectorId");


--
-- Name: IX_UserBuisnesses_UserId_BuisnessId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_UserBuisnesses_UserId_BuisnessId" ON public."UserBuisnesses" USING btree ("UserId", "BuisnessId");


--
-- Name: IX_UserDreamExpectations_DreamId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserDreamExpectations_DreamId" ON public."UserDreamExpectations" USING btree ("DreamId");


--
-- Name: IX_UserKnowledges_KnowledgeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserKnowledges_KnowledgeId" ON public."UserKnowledges" USING btree ("KnowledgeId");


--
-- Name: IX_UserProperties_PropertyId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserProperties_PropertyId" ON public."UserProperties" USING btree ("PropertyId");


--
-- Name: IX_UserStaffs_StaffId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserStaffs_StaffId" ON public."UserStaffs" USING btree ("StaffId");


--
-- Name: IX_UserWorkPositions_WorkPositionWorkId_WorkPositionExpirience~; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserWorkPositions_WorkPositionWorkId_WorkPositionExpirience~" ON public."UserWorkPositions" USING btree ("WorkPositionWorkId", "WorkPositionExpirienceRequire");


--
-- Name: IX_Users_AccountChatId_RoomId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Users_AccountChatId_RoomId" ON public."Users" USING btree ("AccountChatId", "RoomId");


--
-- Name: IX_Users_DreamId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Users_DreamId" ON public."Users" USING btree ("DreamId");


--
-- Name: IX_Users_RoomId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Users_RoomId" ON public."Users" USING btree ("RoomId");


--
-- Name: BuisnessManagerStaffs FK_BuisnessManagerStaffs_ManagerStaffs_ManagerStaffId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BuisnessManagerStaffs"
    ADD CONSTRAINT "FK_BuisnessManagerStaffs_ManagerStaffs_ManagerStaffId" FOREIGN KEY ("ManagerStaffId") REFERENCES public."ManagerStaffs"("Id") ON DELETE CASCADE;


--
-- Name: BuisnessManagerStaffs FK_BuisnessManagerStaffs_UserBuisnesses_UserBuisnessId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BuisnessManagerStaffs"
    ADD CONSTRAINT "FK_BuisnessManagerStaffs_UserBuisnesses_UserBuisnessId" FOREIGN KEY ("UserBuisnessId") REFERENCES public."UserBuisnesses"("Id") ON DELETE CASCADE;


--
-- Name: BuisnessRegionalDirectors FK_BuisnessRegionalDirectors_RegionalDirectors_RegionalDirecto~; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BuisnessRegionalDirectors"
    ADD CONSTRAINT "FK_BuisnessRegionalDirectors_RegionalDirectors_RegionalDirecto~" FOREIGN KEY ("RegionalDirectorId") REFERENCES public."RegionalDirectors"("Id") ON DELETE CASCADE;


--
-- Name: BuisnessRegionalDirectors FK_BuisnessRegionalDirectors_UserBuisnesses_UserBuisnessId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BuisnessRegionalDirectors"
    ADD CONSTRAINT "FK_BuisnessRegionalDirectors_UserBuisnesses_UserBuisnessId" FOREIGN KEY ("UserBuisnessId") REFERENCES public."UserBuisnesses"("Id") ON DELETE CASCADE;


--
-- Name: KnowledgeForWork FK_KnowledgeForWork_Knowledges_KnowledgeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."KnowledgeForWork"
    ADD CONSTRAINT "FK_KnowledgeForWork_Knowledges_KnowledgeId" FOREIGN KEY ("KnowledgeId") REFERENCES public."Knowledges"("Id") ON DELETE CASCADE;


--
-- Name: KnowledgeForWork FK_KnowledgeForWork_Works_WorkId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."KnowledgeForWork"
    ADD CONSTRAINT "FK_KnowledgeForWork_Works_WorkId" FOREIGN KEY ("WorkId") REFERENCES public."Works"("Id") ON DELETE CASCADE;


--
-- Name: SetupCharacterCards FK_SetupCharacterCards_SetupCharacters_SetupCharacterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."SetupCharacterCards"
    ADD CONSTRAINT "FK_SetupCharacterCards_SetupCharacters_SetupCharacterId" FOREIGN KEY ("SetupCharacterId") REFERENCES public."SetupCharacters"("Id") ON DELETE CASCADE;


--
-- Name: UserAccidents FK_UserAccidents_Accidents_AccidentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserAccidents"
    ADD CONSTRAINT "FK_UserAccidents_Accidents_AccidentId" FOREIGN KEY ("AccidentId") REFERENCES public."Accidents"("Id") ON DELETE CASCADE;


--
-- Name: UserAccidents FK_UserAccidents_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserAccidents"
    ADD CONSTRAINT "FK_UserAccidents_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserBuisnesses FK_UserBuisnesses_Buisnesses_BuisnessId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserBuisnesses"
    ADD CONSTRAINT "FK_UserBuisnesses_Buisnesses_BuisnessId" FOREIGN KEY ("BuisnessId") REFERENCES public."Buisnesses"("Id") ON DELETE CASCADE;


--
-- Name: UserBuisnesses FK_UserBuisnesses_FinancialDirectors_FinancialDirectorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserBuisnesses"
    ADD CONSTRAINT "FK_UserBuisnesses_FinancialDirectors_FinancialDirectorId" FOREIGN KEY ("FinancialDirectorId") REFERENCES public."FinancialDirectors"("Id");


--
-- Name: UserBuisnesses FK_UserBuisnesses_GeneralDirectors_GeneralDirectorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserBuisnesses"
    ADD CONSTRAINT "FK_UserBuisnesses_GeneralDirectors_GeneralDirectorId" FOREIGN KEY ("GeneralDirectorId") REFERENCES public."GeneralDirectors"("Id");


--
-- Name: UserBuisnesses FK_UserBuisnesses_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserBuisnesses"
    ADD CONSTRAINT "FK_UserBuisnesses_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserDreamExpectations FK_UserDreamExpectations_Dreams_DreamId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserDreamExpectations"
    ADD CONSTRAINT "FK_UserDreamExpectations_Dreams_DreamId" FOREIGN KEY ("DreamId") REFERENCES public."Dreams"("Id") ON DELETE CASCADE;


--
-- Name: UserDreamExpectations FK_UserDreamExpectations_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserDreamExpectations"
    ADD CONSTRAINT "FK_UserDreamExpectations_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserKnowledges FK_UserKnowledges_Knowledges_KnowledgeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserKnowledges"
    ADD CONSTRAINT "FK_UserKnowledges_Knowledges_KnowledgeId" FOREIGN KEY ("KnowledgeId") REFERENCES public."Knowledges"("Id") ON DELETE CASCADE;


--
-- Name: UserKnowledges FK_UserKnowledges_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserKnowledges"
    ADD CONSTRAINT "FK_UserKnowledges_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserProperties FK_UserProperties_Properties_PropertyId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserProperties"
    ADD CONSTRAINT "FK_UserProperties_Properties_PropertyId" FOREIGN KEY ("PropertyId") REFERENCES public."Properties"("Id") ON DELETE CASCADE;


--
-- Name: UserProperties FK_UserProperties_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserProperties"
    ADD CONSTRAINT "FK_UserProperties_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserStaffs FK_UserStaffs_Staffs_StaffId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserStaffs"
    ADD CONSTRAINT "FK_UserStaffs_Staffs_StaffId" FOREIGN KEY ("StaffId") REFERENCES public."Staffs"("Id") ON DELETE CASCADE;


--
-- Name: UserStaffs FK_UserStaffs_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserStaffs"
    ADD CONSTRAINT "FK_UserStaffs_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserWorkPositions FK_UserWorkPositions_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserWorkPositions"
    ADD CONSTRAINT "FK_UserWorkPositions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserWorkPositions FK_UserWorkPositions_WorkPositions_WorkPositionWorkId_WorkPosi~; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserWorkPositions"
    ADD CONSTRAINT "FK_UserWorkPositions_WorkPositions_WorkPositionWorkId_WorkPosi~" FOREIGN KEY ("WorkPositionWorkId", "WorkPositionExpirienceRequire") REFERENCES public."WorkPositions"("WorkId", "ExpirienceRequire") ON DELETE CASCADE;


--
-- Name: Users FK_Users_Accounts_AccountChatId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "FK_Users_Accounts_AccountChatId" FOREIGN KEY ("AccountChatId") REFERENCES public."Accounts"("ChatId") ON DELETE CASCADE;


--
-- Name: Users FK_Users_Dreams_DreamId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "FK_Users_Dreams_DreamId" FOREIGN KEY ("DreamId") REFERENCES public."Dreams"("Id");


--
-- Name: Users FK_Users_Rooms_RoomId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "FK_Users_Rooms_RoomId" FOREIGN KEY ("RoomId") REFERENCES public."Rooms"("Id") ON DELETE CASCADE;


--
-- Name: VictoryConditions FK_VictoryConditions_Rooms_RoomId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."VictoryConditions"
    ADD CONSTRAINT "FK_VictoryConditions_Rooms_RoomId" FOREIGN KEY ("RoomId") REFERENCES public."Rooms"("Id") ON DELETE CASCADE;


--
-- Name: WorkPositions FK_WorkPositions_Works_WorkId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."WorkPositions"
    ADD CONSTRAINT "FK_WorkPositions_Works_WorkId" FOREIGN KEY ("WorkId") REFERENCES public."Works"("Id") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

--
-- PostgreSQL database cluster dump complete
--

