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
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/1.jpg",
                    Trailer = "https://www.youtube.com/embed/xuZnmYjAKww?si=B_1rowxqE-bt2Azm",
                    GeneroId = 4, //Comedia
                },

                new Peliculas
                {
                    PeliculaId = 2,
                    Titulo = "¿Qué le pasó a Lunes?",
                    Sinopsis = "En un futuro prohibido tener más de un hijo, seis hermanas fingen ser la misma para escapar del Gobierno y buscar a la séptima hermana desaparecida.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/3.jpg",
                    Trailer = "https://www.youtube.com/embed/sRIYLZJqbEY?si=dlF8jxhUUyv8ve44",
                    GeneroId = 3, //Ciencia ficcion
                },

                new Peliculas
                {
                    PeliculaId = 3,
                    Titulo = "Línea de fuego",
                    Sinopsis = "Un exagente de la DEA regresa a la acción para salvar a su hija y a su nueva ciudad de un psicópata que vende drogas.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/2.jpg",
                    Trailer = "https://www.youtube.com/embed/zHrnF621ggQ?si=1W12NksFbJoHon5h",
                    GeneroId = 1, // Accion
                },

                new Peliculas
                {
                    PeliculaId = 4,
                    Titulo = "El robo de mil millones de dolares",
                    Sinopsis = "Un documental sobre la ciberseguridad y se sumerge en uno de los mayores ciberataques, el objetivo era apropiarse de casi mil millones de dólares. En 2016, el Banco Central de Bangladés sufrió un robo de 81 millones de dólares.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/12.png",
                    Trailer = "https://www.youtube.com/embed/4zv56MUXgw8?si=LTcF5VcgHoJPHXIx",
                    GeneroId = 5, //Documental
                },

                new Peliculas
                {
                    PeliculaId = 5,
                    Titulo = "Mean Girls",
                    Sinopsis = "Cady Heron ha sido criada en la selva africana. Sus padres zoólogos intentaron educarla en las leyes de la naturaleza, pero al cumplir quince años, debe ir al instituto y adaptarse a su nueva vida en Illinois.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/20.jpg",
                    Trailer = "https://www.youtube.com/embed/oDU84nmSDZY?si=MVVMaEMdESg8o1Kb",
                    GeneroId = 4,
                },

                new Peliculas
                {
                    PeliculaId = 6,
                    Titulo = "M3GAN",
                    Sinopsis = "M3GAN es una maravilla de la inteligencia artificial, una muñeca realista diseñada para ser la mejor compañera de los niños. Puede escuchar, observar y aprender, convirtiéndose en amiga, profesora, compañera de juegos y protectora.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/4.jpg",
                    Trailer = "https://www.youtube.com/embed/1vTbhymPSA4?si=EM1PU2IF8LLh4F6u",
                    GeneroId = 10,
                },

                new Peliculas
                {
                    PeliculaId = 7,
                    Titulo = "Avengers: Endgame",
                    Sinopsis = "Los héroes de Marvel deben reunirse para deshacer las acciones de Thanos y restaurar el equilibrio en el universo.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/5.jpg",
                    Trailer = "https://www.youtube.com/embed/PyakRSni-c0?si=0OV3qUOuugoO7CRe",
                    GeneroId = 1, // Accion
                },

                new Peliculas
                {
                    PeliculaId = 8,
                    Titulo = "Joker",
                    Sinopsis = "Un cómico fallido se sumerge en la locura y se convierte en el infame villano Joker en Gotham City.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/8.jpg",
                    Trailer = "https://www.youtube.com/embed/zAGVQLHvwOY?si=eI_zYZiB0ZXTV5O9",
                    GeneroId = 9, //Suspense
                },

                new Peliculas
                {
                    PeliculaId = 9,
                    Titulo = "Toy Story",
                    Sinopsis = "Los juguetes de Andy, encabezados por Woody el vaquero, cobran vida cuando nadie está mirando. Pero cuando un nuevo juguete, Buzz Lightyear, llega al grupo, Woody se siente amenazado y se embarcan en una aventura para regresar a casa.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/16.jpg",
                    Trailer = "https://www.youtube.com/embed/v-PjgYDrg70?si=72kWW7Tu48CXLl-y",
                    GeneroId = 7, //Fantasia
                },

                new Peliculas
                {
                    PeliculaId = 10,
                    Titulo = "The Shawshank Redemption",
                    Sinopsis = "Un banquero es condenado por un asesinato que no cometió y pasa décadas en la prisión de Shawshank, donde forma una amistad única con otro recluso.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/14.jpg",
                    Trailer = "https://www.youtube.com/embed/6hB3S9bIaco?si=LpxP7ydPBl8ZP1DD",
                    GeneroId = 6, //Drama
                },

                new Peliculas
                {
                    PeliculaId = 11,
                    Titulo = "La La Land",
                    Sinopsis = "Un pianista y una actriz en ciernes se enamoran en Los Ángeles mientras persiguen sus sueños artísticos.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/9.jpg",
                    Trailer = "https://www.youtube.com/embed/0pdqf4P9MB8?si=Ir5WyILQ-a52fUJ_",
                    GeneroId = 8, //Musical
                },

                new Peliculas
                {
                    PeliculaId = 12,
                    Titulo = "Blackfish",
                    Sinopsis = "Este documental revela la verdad detrás de la cautividad de las orcas en parques acuáticos y la peligrosidad de mantenerlas en cautiverio.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/10.jpg",
                    Trailer = "https://www.youtube.com/embed/fLOeH-Oq_1Y?si=lIAPr6j9_Rc6HN4_",
                    GeneroId = 5, //Documental
                },

                new Peliculas
                {
                    PeliculaId = 13,
                    Titulo = "Indiana Jones and the Last Crusade",
                    Sinopsis = "El intrépido arqueólogo Indiana Jones se embarca en una búsqueda para encontrar el Santo Grial antes que los nazis.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/11.jpg",
                    Trailer = "https://www.youtube.com/embed/a6JB2suJYHM?si=p1jpxsFtBUxDlHec",
                    GeneroId = 2, //Aventuras
                },

                new Peliculas
                {
                    PeliculaId = 14,
                    Titulo = "Viaje al centro de la tierra",
                    Sinopsis = "Un científico descubre una forma de viajar al centro de la Tierra y se embarca en una emocionante aventura subterránea junto a su sobrino y un guía local.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/7.jpg",
                    Trailer = "https://www.youtube.com/embed/ytDcArTr0CI?si=oLr3k3JJJnzDxJt7",
                    GeneroId = 2,
                },

                new Peliculas
                {
                    PeliculaId = 15,
                    Titulo = "Jurassic Park",
                    Sinopsis = "Un grupo de personas queda atrapado en una isla donde se han creado dinosaurios clonados y deben luchar por su supervivencia.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/19.jpg",
                    Trailer = "https://www.youtube.com/embed/lc0UehYemQA?si=AqKnMQbCu6-z9ksl",
                    GeneroId = 2,
                },

                new Peliculas
                {
                    PeliculaId = 16,
                    Titulo = "El Conjuro",
                    Sinopsis = "Basada en hechos reales, una familia experimenta fenómenos paranormales en su nueva casa y busca ayuda de investigadores paranormales.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/6.jpg",
                    Trailer = "https://www.youtube.com/embed/k10ETZ41q5o?si=wcOGoxAW75KvvSXN",
                    GeneroId = 10,
                },

                new Peliculas
                {
                    PeliculaId = 17,
                    Titulo = "Gone Girl",
                    Sinopsis = "Un hombre se convierte en el principal sospechoso cuando su esposa desaparece y las evidencias sugieren que él podría estar involucrado.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/13.jpg",
                    Trailer = "https://www.youtube.com/embed/2-_-1nJf8Vg?si=NSvUWL3POqqQcO1H",
                    GeneroId = 9,
                },

                new Peliculas
                {
                    PeliculaId = 18,
                    Titulo = "Les Misérables",
                    Sinopsis = "La historia de personajes cuyas vidas se entrelazan en la Francia del siglo XIX, luchando por la justicia y la redención.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/15.jpg",
                    Trailer = "https://www.youtube.com/embed/IuEFm84s4oI?si=ub4HSHstEu7Ks4ey",
                    GeneroId = 8, //Musical
                },

                new Peliculas
                {
                    PeliculaId = 19,
                    Titulo = "March of the Penguins",
                    Sinopsis = "Este documental sigue la epopeya de los pingüinos emperadores mientras luchan por sobrevivir y criar a sus crías en la Antártida.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/17.jpg",
                    Trailer = "https://www.youtube.com/embed/ohL8rF_jluA?si=mfdXrh9HUpahNLZu",
                    GeneroId = 5, //Documental
                },

                new Peliculas
                {
                    PeliculaId = 20,
                    Titulo = "Contrarreloj",
                    Sinopsis = "Matt Turner, un hombre de negocios estadounidense que vive en Berlín y que se encuentra en una carrera contrarreloj para salvar a su familia y su propia vida.",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/18.jpg",
                    Trailer = "https://www.youtube.com/embed/T7HlHI4S-MY?si=nwEqiLtSYPzexW8x",
                    GeneroId = 6, //Drama
                }
            );
        }
    }
}
