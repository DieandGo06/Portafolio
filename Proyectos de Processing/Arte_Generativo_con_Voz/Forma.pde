//========================================
//Contiene: class Forma y class Rectangulo
//========================================

class Forma
{
  ArrayList<Rectangulo> rectangulos;
  float[] position = new float[2];

  int linea1_size;
  int linea2_size;
  int size;

  String tipo;
  boolean isPreview;
  boolean isLinea1Vertical;
  
  private Celda celda = tabla.celdas.get(0);
  private Trazo trazo;



  //---------------------------------
  Forma(float positionX_, float positionY_)
  {
    isPreview = true;
    position[x] = positionX_;
    position[y] = positionY_;
    rectangulos = new ArrayList<Rectangulo>();
    fill(color(0, 0, 0, 1));
  }


  //---------------------------------
  void CrearTrazo() 
  {
    trazo = new Trazo(this);
  }


  //---------------------------------
  void DibujarTrazo() 
  {
    trazo.GetFiguraSize(linea1_size, linea2_size);
    trazo.Display();
  }


  //---------------------------------
  boolean OnCollision (Tabla _tabla) 
  {
    for (Rectangulo _rect : rectangulos)
      if (_rect.OnCollision(_tabla)) return true;
    return false;
  }



  //--------------------------------- BASE DE LAS FIGURAS ---------------------------------
  void DibujarPrimeraFigura(int cantidadCeldas)
  {
    float _posX, _posY;
    for (int i = 0; i < cantidadCeldas; i++)
    {
      if (isLinea1Vertical) {
        _posX =  position[x];
        _posY =  position[y] + i * celda.size[y];
      } else {
        _posX =  position[x] + i * celda.size[x];
        _posY =  position[y];
      }

      rectangulos.add(new Rectangulo(_posX, _posY));
      rectangulos.get(i).Dibujar();
    }
  }

  void DibujarSegundaFigura(int cantidadCeldas, String direccion)
  {
    //Si la forma1 es vertical, la forma2 es horizontal.
    //Si la forma2 es horizontal puede ir a la izquierda o derecha. Si es vertical, solo hacia abajo.
    float posX, posY;
    for (int i = 0; i < cantidadCeldas-1; i++)
    {
      if (isLinea1Vertical) 
      {
        if (direccion == "der") posX =  (position[x] + celda.size[x]) + i * celda.size[x];
        else posX =  (position[x] - celda.size[x]) - i * celda.size[x];
        posY =  (position[y] - celda.size[y]) + celda.size[y] * linea1_size;//AltoForma1
      } else {
        posX =  (position[x] - celda.size[x]) + celda.size[x] * linea1_size;//AnchoForma1
        posY =  (position[y] + celda.size[y]) + i * celda.size[y];
      }

      rectangulos.add(new Rectangulo(posX, posY));
      rectangulos.get(linea1_size + i).Dibujar();
    }
  }




  //------------------------------ FORMAS SIMPLES ------------------------------
  void RectaVertical(int _celdasFigura1)
  {
    tipo = "vertical";
    isLinea1Vertical = true;
    linea1_size = _celdasFigura1;
    linea2_size = 0;
    DibujarPrimeraFigura(_celdasFigura1);
  }

  void RectaHorizontal(int _celdasFigura1)
  {
    tipo = "horizontal";
    isLinea1Vertical = false;
    linea1_size = _celdasFigura1;
    linea2_size = 0;
    DibujarPrimeraFigura(_celdasFigura1);
  }



  //------------------------------ FORMAS COMPUESTAS ------------------------------
  void LNormal(int _celdasFigura1, int _celdasFigura2)
  {
    tipo = "L-normal";
    isLinea1Vertical = true;
    linea1_size = _celdasFigura1;
    linea2_size = _celdasFigura2;

    DibujarPrimeraFigura(_celdasFigura1);
    DibujarSegundaFigura(_celdasFigura2, "der");
  }

  void LInvertida(int _celdasFigura1, int _celdasFigura2)
  {
    tipo = "L-invertida";
    isLinea1Vertical = true;
    linea1_size = _celdasFigura1;
    linea2_size = _celdasFigura2;

    DibujarPrimeraFigura(_celdasFigura1);
    DibujarSegundaFigura(_celdasFigura2, "izq");
  }

  void LAcostada(int _celdasFigura1, int _celdasFigura2)
  {
    tipo = "L-acostada";
    isLinea1Vertical = false;
    linea1_size = _celdasFigura1;
    linea2_size = _celdasFigura2;

    DibujarPrimeraFigura(_celdasFigura1);
    DibujarSegundaFigura(_celdasFigura2, null);
  }
}





//===================================================== CLASE RECTANGULO =====================================================
class Rectangulo
{
  float[] vertice1 = new float[2];
  float[] size = new float[2];
  int numeroDeCelda;


  //---------------------------------
  Rectangulo(float posX, float posY)
  {
    vertice1[x] = posX;
    vertice1[y] = posY;
    size[x] = width/tabla.filas;
    size[y] = tabla.alto/tabla.columnas;
  }


  //---------------------------------
  void Dibujar()
  {
    pushStyle();
    noStroke();
    //fill(fondo.colorTrazo);
    //rect(vertice1[x], vertice1[y], size[x], size[y]);
    popStyle();
  }

  //---------------------------------
  boolean OnCollision (Tabla _tabla)
  {
    for (Celda _celda : _tabla.celdas)
      if (vertice1[x] == _celda.vertice1[x]  &&  vertice1[y] == _celda.vertice1[y])
        if (_celda.ocupada == true) 
        {
          return true;
        }
    return false;
  }
}
