using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductosConDB
{
    class Producto
    {
        int codigo;
        string detalle;
        string marca;
        int tipo;
        double precio;
        DateTime fecha;

        public int pCodigo {
            set { codigo = value; }
            get { return codigo; }
        }
        public string pDetalle
        {
            set { detalle = value; }
            get { return detalle; }
        }
        public string pMarca
        {
            set { marca = value; }
            get { return marca; }
        }
        public int pTipo
        {
            set { tipo = value; }
            get { return tipo; }
        }
        public double pPrecio
        {
            set { precio = value; }
            get { return precio; }
        }
        public DateTime pFecha
        {
            set { fecha = value; }
            get { return fecha; }
        }

        public Producto() {
            this.codigo = 0;
            this.detalle = "";
            this.tipo = 0;
            this.marca = "";
            this.precio = 0;
            this.fecha = DateTime.Today;
        }
        public Producto(int codigo, string detalle, int tipo, string marca, double precio, DateTime fecha)
        {
            this.codigo = codigo;
            this.detalle = detalle;
            this.tipo = tipo;
            this.marca = marca;
            this.precio = precio;
            this.fecha = fecha;
        }

        public string toString() {
            return codigo + " - " + detalle;

        }

    }
}
