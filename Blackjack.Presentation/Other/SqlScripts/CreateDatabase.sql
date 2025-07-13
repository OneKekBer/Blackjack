CREATE TABLE IF NOT EXISTS public."Games"
(
    "Id" uuid NOT NULL,
    -- Add other Game columns here
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "PK_Games" PRIMARY KEY ("Id")
    ) TABLESPACE pg_default;

ALTER TABLE public."Games" OWNER TO postgres;

CREATE TABLE IF NOT EXISTS public."PlayerConnections"
(
    "Id" uuid NOT NULL,
    "PlayerId" uuid NOT NULL,
    "ConnectionId" text COLLATE pg_catalog."default" NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "PK_PlayerConnections" PRIMARY KEY ("Id")
    ) TABLESPACE pg_default;

ALTER TABLE public."PlayerConnections" OWNER TO postgres;

CREATE TABLE IF NOT EXISTS public."Players"
(
    "Id" uuid NOT NULL,
    "IsPlaying" boolean NOT NULL,
    "Role" integer NOT NULL,
    "Name" text COLLATE pg_catalog."default" NOT NULL,
    "Balance" integer NOT NULL,
    "Cards" text COLLATE pg_catalog."default" NOT NULL,
    "UserId" uuid,
    "GameEntityId" uuid,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "PK_Players" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Players_Games_GameEntityId" FOREIGN KEY ("GameEntityId")
    REFERENCES public."Games" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    ) TABLESPACE pg_default;

ALTER TABLE public."Players" OWNER TO postgres;

CREATE INDEX IF NOT EXISTS "IX_Players_GameEntityId"
    ON public."Players" USING btree
    ("GameEntityId" ASC NULLS LAST)
    TABLESPACE pg_default;