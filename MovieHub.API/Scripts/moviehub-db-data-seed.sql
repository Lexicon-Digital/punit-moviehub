/*
==================================================================
Notes
==================================================================

Below you will find the insert statements for the tables Cinema, Movies and MovieCinema
Once the inserts are executed, the data can be verified using the following query:

SELECT m.title AS "Movie Title", c.name AS "Cinema", mc.ticketPrice AS "Ticket Price"
  FROM Movie m
  JOIN MovieCinema mc ON mc.movieId = m.id
  JOIN Cinema c ON c.id = mc.cinemaId;
*/

-- ======================== INSERT Cinema ========================
INSERT INTO Cinema (name, location) VALUES 
  ("Cinemarvel", "72 Bette McNee Street, Sandy Gully, NSW 2729"), 
  ("Moviemania", "7 Old Tenterfield Road, Old Bonalbo, NSW 2469"),
  ("BigScreen Bliss", "93 Creek Street, Kooralgin, Queensland 4402"),
  ("CineNova", "59 McLachlan Street, Nurcoung, Victoria 3401"),
  ("Cinema Royale", "89 Nerrigundah Drive, Junction Village, Victoria 3977"),
  ("Flicker Factory", "96 Sale-Heyfield Road, Kardella, Victoria 3951"),
  ("CineSpectra", "28 Reynolds Road, Lake Borumba, Queensland 4570");

-- ======================== INSERT Movie =========================
INSERT INTO Movie (title, releaseDate, genre, runtime, synopsis, director, rating, princessTheatreMovieId) VALUES 
  (
    "Star Wars: The Phantom Menace (Episode I)", 
    "1999-05-19",
    "Action, Adventure, Fantasy, Live Action, Science Fiction",
    8160,
    "Experience the heroic action and unforgettable adventures of Star Wars: Episode I - The Phantom Menace. See the first fateful steps in the journey of Anakin Skywalker. Stranded on the desert planet Tatooine after rescuing young Queen Amidala from the impending invasion of Naboo, Jedi apprentice Obi-Wan Kenobi and his Jedi Master Qui-Gon Jinn discover nine-year-old Anakin, who is unusually strong in the Force. Anakin wins a thrilling Podrace and with it his freedom as he leaves his home to be trained as a Jedi. The heroes return to Naboo where Anakin and the Queen face massive invasion forces while the two Jedi contend with a deadly foe named Darth Maul. Only then do they realize the invasion is merely the first step in a sinister scheme by the re-emergent forces of darkness known as the Sith.",
    "George Lucas",
    "PG",
    "0120915"
  ),
  (
    "Star Wars: Attack of the Clones (Episode II)", 
    "2002-05-16",
    "Action, Adventure, Fantasy, Live Action, Science Fiction",
    8520,
    "Watch the seeds of Anakin Skywalker's transformation take root in Star Wars: Episode II - Attack of the Clones. Ten years after the invasion of Naboo, the galaxy is on the brink of civil war. Under the leadership of a renegade Jedi named Count Dooku, thousands of solar systems threaten to break away from the Galactic Republic. When an assassination attempt is made on Senator Padmé Amidala, the former Queen of Naboo, twenty-year-old Jedi apprentice Anakin Skywalker is assigned to protect her. In the course of his mission, Anakin discovers his love for Padmé as well as his own darker side. Soon, Anakin, Padmé, and Obi-Wan Kenobi are drawn into the heart of the Separatist movement and the beginning of the Clone Wars.",
    "George Lucas",
    "PG-13",
    "0121765"
  ),
  (
    "Star Wars: Revenge of the Sith (Episode III)", 
    "2005-05-19",
    "Action, Adventure, Fantasy, Live Action, Science Fiction",
    8399,
    "Discover the true power of the dark side in Star Wars: Episode III - Revenge of the Sith. Years after the onset of the Clone Wars, the noble Jedi Knights lead a massive clone army into a galaxy-wide battle against the Separatists. When the sinister Sith unveil a thousand-year-old plot to rule the galaxy, the Republic crumbles and from its ashes rises the evil Galactic Empire. Jedi hero Anakin Skywalker is seduced by the dark side of the Force to become the Emperor's new apprentice – Darth Vader. The Jedi are decimated, as Obi-Wan Kenobi and Jedi Master Yoda are forced into hiding.",
    "George Lucas",
    "PG-13",
    "0121766"
  ),
  (
    "Star Wars: A New Hope (Episode IV)", 
    "1977-10-27",
    "Action, Adventure, Fantasy, Live Action, Science Fiction",
    6300,
    "Luke Skywalker begins a journey that will change the galaxy in Star Wars: Episode IV - A New Hope. Nineteen years after the formation of the Empire, Luke is thrust into the struggle of the Rebel Alliance when he meets Obi-Wan Kenobi, who has lived for years in seclusion on the desert planet of Tatooine. Obi-Wan begins Luke's Jedi training as Luke joins him on a daring mission to rescue the beautiful Rebel leader Princess Leia from the clutches of Darth Vader and the evil Empire.",
    "George Lucas",
    "PG",
    "0076759"
  ),
  (
    "Star Wars: The Empire Strikes Back (Episode V)", 
    "1980-08-07",
    "Action, Adventure, Fantasy, Live Action, Science Fiction",
    7440,
    "After the destruction of the Death Star, Imperial forces continue to pursue the Rebels. After the Rebellion's defeat on the ice planet Hoth, Luke journeys to the planet Dagobah to train with Jedi Master Yoda, who has lived in hiding since the fall of the Republic. In an attempt to convert Luke to the dark side, Darth Vader lures young Skywalker into a trap in the Cloud City of Bespin.",
    "George Lucas",
    "PG",
    "0080684"
  ),
  (
    "Star Wars: Return of the Jedi (Episode VI)", 
    "1983-10-27",
    "Action, Adventure, Fantasy, Live Action, Science Fiction",
    7859,
    "After a quick trip back to Tatooine, Luke Skywalker, Leia Organa, and Han Solo are reunited and join up with the amassing rebel fleet to take down the evil Empire once and for all. But the Empire is plotting too. Emperor Palpatine commands his troops aboard his newly consturcted Death Star stationed above the forest moon of Endor, where the rebels - and some unlikely furry friends - make their stand against the Empire. While Luke Skywalker confronts Darth Vader on the Death Star once more, Han leads a team to take out a shield protecting the battle station so that the rebel fleet can destory it once more and finally put an end to the war.",
    "George Lucas",
    "PG",
    "0086190"
  ),
  (
    "Star Wars: The Force Awakens (Episode VII)", 
    "2015-12-18",
    "Action - Adventure, Science Fiction",
    8160,
    "Thirty years since the destruction of the second Death Star, the sinister First Order, commanded by the mysterious Snoke and apprentice Kylo Ren, rise from the ashes of the Empire. The Resistance, led by General Leia Organa, attempts to thwart the First Order's threat, but they're desperate for help. Rey, a desert scavenger, and Finn, an ex-stormtrooper, find themselves joining forces with Han Solo and Chewbacca in a desperate mission to return a BB-unit droid with a map to Luke Skywalker back to the Resistance.",
    "George Lucas",
    "PG-13",
    "2488496"
  ),
  (
    "Star Wars: The Last Jedi (Episode VIII)", 
    "2017-12-13",
    "Action - Adventure, Science Fiction",
    9000,
    "The Resistance is in desperate need of help when they find themselves impossibly pursued by the First Order. While Rey travels to a remote planet called Ahch-To to recruit Luke Skywalker to the Resistance, Finn and Rose, a mechanic, go on their own mission in the hopes of helping the Resistance finally escape the First Order. But everyone finds themselves on the salt-planet of Crait for a last stand.",
    "George Lucas",
    "PG-13",
    "2527336"
  ),
  (
    "Star Wars: The Rise of Skywalker (Episode IX)", 
    "2019-12-18",
    "Action - Adventure, Science Fiction",
    7992,
    "Lucasfilm and director J.J. Abrams join forces once  more to take viewers on an epic journey to a galaxy far, far away with Star Wars: The Rise of Skywalker, the riveting conclusion of the  landmark Skywalker saga, in which new legends will be born and the final battle for freedom is yet to come.",
    "George Lucas",
    "PG-13",
    "2527338"
  );

-- ======================== INSERT MovieCinema =========================
INSERT INTO MovieCinema (movieId, cinemaId, showtime, ticketPrice) VALUES
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Phantom Menace (Episode I)"),
  (SELECT id FROM Cinema WHERE name = "Cinemarvel"),
  "2024-12-31",
  24.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Phantom Menace (Episode I)"),
  (SELECT id FROM Cinema WHERE name = "Moviemania"),
  "2024-12-31",
  22.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Phantom Menace (Episode I)"),
  (SELECT id FROM Cinema WHERE name = "BigScreen Bliss"),
  "2024-12-31",
  20.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Phantom Menace (Episode I)"),
  (SELECT id FROM Cinema WHERE name = "CineNova"),
  "2024-12-31",
  21.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Phantom Menace (Episode I)"),
  (SELECT id FROM Cinema WHERE name = "Cinema Royale"),
  "2024-12-31",
  22.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Phantom Menace (Episode I)"),
  (SELECT id FROM Cinema WHERE name = "Flicker Factory"),
  "2024-12-31",
  23.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Phantom Menace (Episode I)"),
  (SELECT id FROM Cinema WHERE name = "CineSpectra"),
  "2024-12-31",
  22.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Attack of the Clones (Episode II)"),
  (SELECT id FROM Cinema WHERE name = "Cinemarvel"),
  "2024-12-31",
  26.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Attack of the Clones (Episode II)"),
  (SELECT id FROM Cinema WHERE name = "Moviemania"),
  "2024-12-31",
  21.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Attack of the Clones (Episode II)"),
  (SELECT id FROM Cinema WHERE name = "BigScreen Bliss"),
  "2024-12-31",
  25.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Attack of the Clones (Episode II)"),
  (SELECT id FROM Cinema WHERE name = "CineNova"),
  "2024-12-31",
  27.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Attack of the Clones (Episode II)"),
  (SELECT id FROM Cinema WHERE name = "Cinema Royale"),
  "2024-12-31",
  22.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Attack of the Clones (Episode II)"),
  (SELECT id FROM Cinema WHERE name = "Flicker Factory"),
  "2024-12-31",
  29.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Attack of the Clones (Episode II)"),
  (SELECT id FROM Cinema WHERE name = "CineSpectra"),
  "2024-12-31",
  23.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Revenge of the Sith (Episode III)"),
  (SELECT id FROM Cinema WHERE name = "Cinemarvel"),
  "2024-12-31",
  21.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Revenge of the Sith (Episode III)"),
  (SELECT id FROM Cinema WHERE name = "Moviemania"),
  "2024-12-31",
  25.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Revenge of the Sith (Episode III)"),
  (SELECT id FROM Cinema WHERE name = "BigScreen Bliss"),
  "2024-12-31",
  29.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Revenge of the Sith (Episode III)"),
  (SELECT id FROM Cinema WHERE name = "CineNova"),
  "2024-12-31",
  20.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Revenge of the Sith (Episode III)"),
  (SELECT id FROM Cinema WHERE name = "Cinema Royale"),
  "2024-12-31",
  27.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Revenge of the Sith (Episode III)"),
  (SELECT id FROM Cinema WHERE name = "Flicker Factory"),
  "2024-12-31",
  26.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Revenge of the Sith (Episode III)"),
  (SELECT id FROM Cinema WHERE name = "CineSpectra"),
  "2024-12-31",
  20.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: A New Hope (Episode IV)"),
  (SELECT id FROM Cinema WHERE name = "Cinemarvel"),
  "2024-12-31",
  28.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: A New Hope (Episode IV)"),
  (SELECT id FROM Cinema WHERE name = "Moviemania"),
  "2024-12-31",
  23.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: A New Hope (Episode IV)"),
  (SELECT id FROM Cinema WHERE name = "BigScreen Bliss"),
  "2024-12-31",
  25.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: A New Hope (Episode IV)"),
  (SELECT id FROM Cinema WHERE name = "CineNova"),
  "2024-12-31",
  28.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: A New Hope (Episode IV)"),
  (SELECT id FROM Cinema WHERE name = "Cinema Royale"),
  "2024-12-31",
  20.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: A New Hope (Episode IV)"),
  (SELECT id FROM Cinema WHERE name = "Flicker Factory"),
  "2024-12-31",
  24.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: A New Hope (Episode IV)"),
  (SELECT id FROM Cinema WHERE name = "CineSpectra"),
  "2024-12-31",
  26.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Empire Strikes Back (Episode V)"),
  (SELECT id FROM Cinema WHERE name = "Cinemarvel"),
  "2024-12-31",
  28.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Empire Strikes Back (Episode V)"),
  (SELECT id FROM Cinema WHERE name = "Moviemania"),
  "2024-12-31",
  24.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Empire Strikes Back (Episode V)"),
  (SELECT id FROM Cinema WHERE name = "BigScreen Bliss"),
  "2024-12-31",
  29.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Empire Strikes Back (Episode V)"),
  (SELECT id FROM Cinema WHERE name = "CineNova"),
  "2024-12-31",
  21.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Empire Strikes Back (Episode V)"),
  (SELECT id FROM Cinema WHERE name = "Cinema Royale"),
  "2024-12-31",
  23.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Empire Strikes Back (Episode V)"),
  (SELECT id FROM Cinema WHERE name = "Flicker Factory"),
  "2024-12-31",
  27.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Empire Strikes Back (Episode V)"),
  (SELECT id FROM Cinema WHERE name = "CineSpectra"),
  "2024-12-31",
  22.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Return of the Jedi (Episode VI)"),
  (SELECT id FROM Cinema WHERE name = "Cinemarvel"),
  "2024-12-31",
  29.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Return of the Jedi (Episode VI)"),
  (SELECT id FROM Cinema WHERE name = "Moviemania"),
  "2024-12-31",
  22.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Return of the Jedi (Episode VI)"),
  (SELECT id FROM Cinema WHERE name = "BigScreen Bliss"),
  "2024-12-31",
  25.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Return of the Jedi (Episode VI)"),
  (SELECT id FROM Cinema WHERE name = "CineNova"),
  "2024-12-31",
  20.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Return of the Jedi (Episode VI)"),
  (SELECT id FROM Cinema WHERE name = "Cinema Royale"),
  "2024-12-31",
  21.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Return of the Jedi (Episode VI)"),
  (SELECT id FROM Cinema WHERE name = "Flicker Factory"),
  "2024-12-31",
  22.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: Return of the Jedi (Episode VI)"),
  (SELECT id FROM Cinema WHERE name = "CineSpectra"),
  "2024-12-31",
  23.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Force Awakens (Episode VII)"),
  (SELECT id FROM Cinema WHERE name = "Cinemarvel"),
  "2024-12-31",
  24.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Force Awakens (Episode VII)"),
  (SELECT id FROM Cinema WHERE name = "Moviemania"),
  "2024-12-31",
  26.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Force Awakens (Episode VII)"),
  (SELECT id FROM Cinema WHERE name = "BigScreen Bliss"),
  "2024-12-31",
  25.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Force Awakens (Episode VII)"),
  (SELECT id FROM Cinema WHERE name = "CineNova"),
  "2024-12-31",
  28.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Force Awakens (Episode VII)"),
  (SELECT id FROM Cinema WHERE name = "Cinema Royale"),
  "2024-12-31",
  20.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Force Awakens (Episode VII)"),
  (SELECT id FROM Cinema WHERE name = "Flicker Factory"),
  "2024-12-31",
  26.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Force Awakens (Episode VII)"),
  (SELECT id FROM Cinema WHERE name = "CineSpectra"),
  "2024-12-31",
  28.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Last Jedi (Episode VIII)"),
  (SELECT id FROM Cinema WHERE name = "Cinemarvel"),
  "2024-12-31",
  27.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Last Jedi (Episode VIII)"),
  (SELECT id FROM Cinema WHERE name = "Moviemania"),
  "2024-12-31",
  23.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Last Jedi (Episode VIII)"),
  (SELECT id FROM Cinema WHERE name = "BigScreen Bliss"),
  "2024-12-31",
  29.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Last Jedi (Episode VIII)"),
  (SELECT id FROM Cinema WHERE name = "CineNova"),
  "2024-12-31",
  20.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Last Jedi (Episode VIII)"),
  (SELECT id FROM Cinema WHERE name = "Cinema Royale"),
  "2024-12-31",
  24.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Last Jedi (Episode VIII)"),
  (SELECT id FROM Cinema WHERE name = "Flicker Factory"),
  "2024-12-31",
  21.25
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Last Jedi (Episode VIII)"),
  (SELECT id FROM Cinema WHERE name = "CineSpectra"),
  "2024-12-31",
  22.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Rise of Skywalker (Episode IX)"),
  (SELECT id FROM Cinema WHERE name = "Cinemarvel"),
  "2024-12-31",
  25.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Rise of Skywalker (Episode IX)"),
  (SELECT id FROM Cinema WHERE name = "Moviemania"),
  "2024-12-31",
  27.75
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Rise of Skywalker (Episode IX)"),
  (SELECT id FROM Cinema WHERE name = "BigScreen Bliss"),
  "2024-12-31",
  20.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Rise of Skywalker (Episode IX)"),
  (SELECT id FROM Cinema WHERE name = "CineNova"),
  "2024-12-31",
  21.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Rise of Skywalker (Episode IX)"),
  (SELECT id FROM Cinema WHERE name = "Cinema Royale"),
  "2024-12-31",
  28.50
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Rise of Skywalker (Episode IX)"),
  (SELECT id FROM Cinema WHERE name = "Flicker Factory"),
  "2024-12-31",
  28.00
),
(
  (SELECT id FROM Movie WHERE title = "Star Wars: The Rise of Skywalker (Episode IX)"),
  (SELECT id FROM Cinema WHERE name = "CineSpectra"),
  "2024-12-31",
  21.25
);
