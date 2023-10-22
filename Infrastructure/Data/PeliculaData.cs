using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data
{
    public class PeliculaData : IEntityTypeConfiguration<Peliculas>
    {
        public void Configure(EntityTypeBuilder<Peliculas> entityBuilder)
        {
            entityBuilder.HasData
            (
                new Peliculas
                {
                    PeliculaId = 1,
                    Titulo = "Una esposa de mentira",
                    Sinopsis = "El cirujano Danny decide contratar a su ayudante Katherine, una madre soltera con hijos, para que finjan ser su familia. Su intención es demostrarle a Palmer que su amor por ella es tan grande que está a punto de divorciarse de su mujer.",
                    Poster = "https://i.imgur.com/Qdd6klb.jpg",
                    Trailer = "https://www.youtube.com/watch?v=xuZnmYjAKww",
                    GeneroId = 4, //Comedia
                },

                new Peliculas
                {
                    PeliculaId = 2,
                    Titulo = "¿Qué le pasó a Lunes?",
                    Sinopsis = "En un futuro prohibido tener más de un hijo, seis hermanas fingen ser la misma para escapar del Gobierno y buscar a la séptima hermana desaparecida.",
                    Poster = "https://i.imgur.com/xz1PBy0.jpg",
                    Trailer = "https://www.youtube.com/watch?v=sRIYLZJqbEY",
                    GeneroId = 3, //Ciencia ficcion
                },

                new Peliculas
                {
                    PeliculaId = 3,
                    Titulo = "Línea de fuego",
                    Sinopsis = "Un exagente de la DEA regresa a la acción para salvar a su hija y a su nueva ciudad de un psicópata que vende drogas.",
                    Poster = "https://i.imgur.com/gWGCYdr.jpg",
                    Trailer = "https://www.youtube.com/watch?v=zHrnF621ggQ",
                    GeneroId = 1, // Accion
                },

                new Peliculas //cambiar pelicula
                {
                    PeliculaId = 4,
                    Titulo = "El robo de mil millones de dolares",
                    Sinopsis = "Un documental sobre la ciberseguridad y se sumerge en uno de los mayores ciberataques, el objetivo era apropiarse de casi mil millones de dólares. En 2016, el Banco Central de Bangladés sufrió un robo de 81 millones de dólares.",
                    Poster = "https://i.imgur.com/Q2eeSOC.png",
                    Trailer = "https://www.youtube.com/watch?v=4zv56MUXgw8",
                    GeneroId = 5, //Documental
                },

                new Peliculas
                {
                    PeliculaId = 5,
                    Titulo = "Mean Girls",
                    Sinopsis = "Cady Heron ha sido criada en la selva africana. Sus padres zoólogos intentaron educarla en las leyes de la naturaleza, pero al cumplir quince años, debe ir al instituto y adaptarse a su nueva vida en Illinois.",
                    Poster = "https://i.imgur.com/VJGTWKG.jpg",
                    Trailer = "https://www.youtube.com/watch?v=oDU84nmSDZY",
                    GeneroId = 4,
                },

                new Peliculas
                {
                    PeliculaId = 6,
                    Titulo = "M3GAN",
                    Sinopsis = "M3GAN es una maravilla de la inteligencia artificial, una muñeca realista diseñada para ser la mejor compañera de los niños. Puede escuchar, observar y aprender, convirtiéndose en amiga, profesora, compañera de juegos y protectora.",
                    Poster = "https://i.imgur.com/HKpufD5.jpg",
                    Trailer = "https://www.youtube.com/watch?v=1vTbhymPSA4",
                    GeneroId = 10,
                },

                new Peliculas
                {
                    PeliculaId = 7,
                    Titulo = "Avengers: Endgame",
                    Sinopsis = "Los héroes de Marvel deben reunirse para deshacer las acciones de Thanos y restaurar el equilibrio en el universo.",
                    Poster = "https://i.imgur.com/tGJwzGc.jpg",
                    Trailer = "https://www.youtube.com/watch?v=PyakRSni-c0",
                    GeneroId = 1, // Accion
                },

                new Peliculas
                {
                    PeliculaId = 8,
                    Titulo = "Joker",
                    Sinopsis = "Un cómico fallido se sumerge en la locura y se convierte en el infame villano Joker en Gotham City.",
                    Poster = "https://i.imgur.com/rRAMOZR.jpg",
                    Trailer = "https://www.youtube.com/watch?v=zAGVQLHvwOY",
                    GeneroId = 9, //Suspense
                },

                new Peliculas
                {
                    PeliculaId = 9,
                    Titulo = "Toy Story",
                    Sinopsis = "Los juguetes de Andy, encabezados por Woody el vaquero, cobran vida cuando nadie está mirando. Pero cuando un nuevo juguete, Buzz Lightyear, llega al grupo, Woody se siente amenazado y se embarcan en una aventura para regresar a casa.",
                    Poster = "https://i.imgur.com/U6aTYJU.jpg",
                    Trailer = "https://www.youtube.com/watch?v=v-PjgYDrg70",
                    GeneroId = 7, //Fantasia
                },

                new Peliculas
                {
                    PeliculaId = 10,
                    Titulo = "The Shawshank Redemption",
                    Sinopsis = "Un banquero es condenado por un asesinato que no cometió y pasa décadas en la prisión de Shawshank, donde forma una amistad única con otro recluso.",
                    Poster = "https://i.imgur.com/333XNA8.jpg",
                    Trailer = "https://www.youtube.com/watch?v=6hB3S9bIaco",
                    GeneroId = 6, //Drama
                },

                new Peliculas
                {
                    PeliculaId = 11,
                    Titulo = "La La Land",
                    Sinopsis = "Un pianista y una actriz en ciernes se enamoran en Los Ángeles mientras persiguen sus sueños artísticos.",
                    Poster = "https://i.imgur.com/Yo52ZWJ.jpg",
                    Trailer = "https://www.youtube.com/watch?v=0pdqf4P9MB8",
                    GeneroId = 8, //Musical
                },

                new Peliculas
                {
                    PeliculaId = 12,
                    Titulo = "Blackfish",
                    Sinopsis = "Este documental revela la verdad detrás de la cautividad de las orcas en parques acuáticos y la peligrosidad de mantenerlas en cautiverio.",
                    Poster = "https://i.imgur.com/ACXFboh.jpg",
                    Trailer = "https://www.youtube.com/watch?v=fLOeH-Oq_1Y",
                    GeneroId = 5, //Documental
                },

                new Peliculas
                {
                    PeliculaId = 13,
                    Titulo = "Indiana Jones and the Last Crusade",
                    Sinopsis = "El intrépido arqueólogo Indiana Jones se embarca en una búsqueda para encontrar el Santo Grial antes que los nazis.",
                    Poster = "https://i.imgur.com/zFvBmAK.jpg",
                    Trailer = "https://www.youtube.com/watch?v=a6JB2suJYHM",
                    GeneroId = 2, //Aventuras
                },

                new Peliculas
                {
                    PeliculaId = 14,
                    Titulo = "Viaje al centro de la tierra",
                    Sinopsis = "Un científico descubre una forma de viajar al centro de la Tierra y se embarca en una emocionante aventura subterránea junto a su sobrino y un guía local.",
                    Poster = "https://i.imgur.com/1oE0Ud8.jpg",
                    Trailer = "https://www.youtube.com/watch?v=ytDcArTr0CI",
                    GeneroId = 2,
                },

                new Peliculas
                {
                    PeliculaId = 15,
                    Titulo = "Jurassic Park",
                    Sinopsis = "Un grupo de personas queda atrapado en una isla donde se han creado dinosaurios clonados y deben luchar por su supervivencia.",
                    Poster = "https://i.imgur.com/xwJNL4Z.jpg",
                    Trailer = "https://www.youtube.com/watch?v=lc0UehYemQA",
                    GeneroId = 2,
                },

                new Peliculas
                {
                    PeliculaId = 16,
                    Titulo = "El Conjuro",
                    Sinopsis = "Basada en hechos reales, una familia experimenta fenómenos paranormales en su nueva casa y busca ayuda de investigadores paranormales.",
                    Poster = "https://i.imgur.com/vG8P2jY.jpg",
                    Trailer = "https://www.youtube.com/watch?v=k10ETZ41q5o",
                    GeneroId = 10,
                },

                new Peliculas
                {
                    PeliculaId = 17,
                    Titulo = "Gone Girl",
                    Sinopsis = "Un hombre se convierte en el principal sospechoso cuando su esposa desaparece y las evidencias sugieren que él podría estar involucrado.",
                    Poster = "https://i.imgur.com/lYGE3LJ.jpg",
                    Trailer = "https://www.youtube.com/watch?v=2-_-1nJf8Vg",
                    GeneroId = 9,
                },

                new Peliculas
                {
                    PeliculaId = 18,
                    Titulo = "Les Misérables",
                    Sinopsis = "La historia de personajes cuyas vidas se entrelazan en la Francia del siglo XIX, luchando por la justicia y la redención.",
                    Poster = "https://i.imgur.com/ZrRH8z5.jpg",
                    Trailer = "https://www.youtube.com/watch?v=IuEFm84s4oI",
                    GeneroId = 8, //Musical
                },

                new Peliculas
                {
                    PeliculaId = 19,
                    Titulo = "March of the Penguins",
                    Sinopsis = "Este documental sigue la epopeya de los pingüinos emperadores mientras luchan por sobrevivir y criar a sus crías en la Antártida.",
                    Poster = "https://i.imgur.com/xRZC4Ji.jpg",
                    Trailer = "https://www.youtube.com/watch?v=ohL8rF_jluA",
                    GeneroId = 5, //Documental
                },

                new Peliculas
                {
                    PeliculaId = 20,
                    Titulo = "Contrarreloj",
                    Sinopsis = "Matt Turner, un hombre de negocios estadounidense que vive en Berlín y que se encuentra en una carrera contrarreloj para salvar a su familia y su propia vida.",
                    Poster = "https://i.imgur.com/LeQf3gP.jpg",
                    Trailer = "https://www.youtube.com/watch?v=T7HlHI4S-MY",
                    GeneroId = 6, //Drama
                }
            );
        }
    }
}
