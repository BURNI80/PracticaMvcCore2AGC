using PracticaMvcCore2AGC.Data;
using PracticaMvcCore2AGC.Models;

namespace PracticaMvcCore2AGC.Repositorys
{
    public class LibrosRepository
    {
        private LibrosContext context;

        public LibrosRepository(LibrosContext context)
        {
            this.context = context;
        }


        public List<Genero> GetGeneros()
        {
            var consulta = this.context.Generos;
            return consulta.ToList();
        }

        public int CountLibros()
        {
            return this.context.Libros.ToList().Count();
        }

        public List<Libro> GetLibros(int pos)
        {
            List<Libro> libros = this.context.Libros.ToList();
            return libros.Skip(pos).Take(5).ToList();
        }

        public List<Libro> GetLibrosGenero(int idGenero)
        {
            return this.context.Libros.Where(x => x.IdGenero == idGenero).ToList();
        }


        public Libro GetLibro(int id)
        {
            return this.context.Libros.Where(x => x.IdLibro == id).FirstOrDefault();
        }

        public List<Libro> GetLibrosCarrito(string stringSession)
        {
            List<Libro> vacio = new List<Libro>();
            if (stringSession == "")
            {
                return vacio;
            }
            if (stringSession != null)
            {
                string[] arr = stringSession.Split(',');
                List<Libro> libros = new List<Libro>();
                for (int i = 0; i < arr.Length; i++)
                {
                    libros.Add(GetLibro(int.Parse(arr[i])));
                }
                return libros;

            }
            return vacio;
        }

        public string DeleteLibrosCarrito(string stringSession, int pos)
        {
            if (stringSession != null)
            {
                string[] arr = stringSession.Split(',');
                arr = arr.Where(w => w != arr[pos]).ToArray();
                var str = String.Join(",", arr);
                return str;
            }
            return null;

        }

        public Usuario ComprobarUser(string username, string pass)
        {
            return this.context.Usuarios.Where(x => x.Email == username && x.Pass == pass).FirstOrDefault();
        }

        public Usuario GetPerfil(int id)
        {
            return this.context.Usuarios.Where(x => x.IdUsuario == id).FirstOrDefault();
        }

        public async Task<int> Comprar(string arr, int idUsua)
        {
            List<Libro> libros = GetLibrosCarrito(arr);
            int nfactura = this.context.Pedidos.Max(x => x.IdFactura) + 1;
            foreach (Libro l in libros)
            {
                int npedido = this.context.Pedidos.Max(x => x.IdPedido) + 1;
                Pedido p = new Pedido();
                p.IdFactura = nfactura;
                p.IdPedido = npedido;
                p.IdLibro = l.IdLibro;
                p.IdUsuario = idUsua;
                p.Fecha = DateTime.Now;
                p.Cantidad = 1;
                this.context.Pedidos.Add(p);
                await this.context.SaveChangesAsync();
            }
            return 0;
        }

        public List<VistaPedido> GetVistaPedidos(int id)
        {
            return this.context.VistaPedidos.Where(x => x.IdUsuario == id).ToList();
        }
    }
}
