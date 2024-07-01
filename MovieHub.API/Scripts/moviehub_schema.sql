CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS "Cinema" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Cinema" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Location" TEXT NOT NULL
);
CREATE TABLE sqlite_sequence(name,seq);
CREATE TABLE IF NOT EXISTS "Movie" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Movie" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "ReleaseDate" TEXT NOT NULL,
    "Genre" TEXT NOT NULL,
    "RunTime" INTEGER NOT NULL,
    "Synopsis" TEXT NOT NULL,
    "Director" TEXT NOT NULL,
    "Rating" TEXT NOT NULL,
    "PrincessTheatreMovieId" TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS "MovieCinema" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_MovieCinema" PRIMARY KEY AUTOINCREMENT,
    "ShowTime" TEXT NOT NULL,
    "TicketPrice" TEXT NOT NULL,
    "MovieId" INTEGER NOT NULL,
    "CinemaId" INTEGER NOT NULL,
    CONSTRAINT "FK_MovieCinema_Cinema_CinemaId" FOREIGN KEY ("CinemaId") REFERENCES "Cinema" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MovieCinema_Movie_MovieId" FOREIGN KEY ("MovieId") REFERENCES "Movie" ("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_MovieCinema_CinemaId" ON "MovieCinema" ("CinemaId");
CREATE INDEX "IX_MovieCinema_MovieId" ON "MovieCinema" ("MovieId");
CREATE TABLE IF NOT EXISTS "MovieReview" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_MovieReview" PRIMARY KEY AUTOINCREMENT,
    "Score" TEXT NOT NULL,
    "Comment" TEXT NULL,
    "ReviewDate" TEXT NOT NULL,
    "MovieId" INTEGER NOT NULL,
    CONSTRAINT "FK_MovieReview_Movie_MovieId" FOREIGN KEY ("MovieId") REFERENCES "Movie" ("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_MovieReview_MovieId" ON "MovieReview" ("MovieId");
