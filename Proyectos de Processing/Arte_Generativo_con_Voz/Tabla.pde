class Tabla 
{
  ArrayList<Celda> celdas;
  int numeroDeCelda;
  boolean mostrar;

  int filas = 10;
  int columnas = 14;
  int alto = 588;
  int ancho = width;
  int alturaInicial = -10;


  //--------------------------------- Tabla Grande
  Tabla() 
  {
    celdas = new ArrayList<Celda>();
    for (int i=0; i < filas * columnas; i++)
      celdas.add(new Celda());
  }

  //--------------------------------- Tabla Pequeña
  Tabla(int _filas, int _columnas, int _ancho, int _alto) 
  {
    filas = _filas;
    columnas = _columnas;
    ancho = _ancho;
    alto = _alto;

    celdas = new ArrayList<Celda>();
    for (int i=0; i < filas * columnas; i++)
      celdas.add(new Celda());
  }



  //--------------------------------- 
  void Crear(int _posX, int _posY)
  {
    for (int i = 0; i < columnas; i++)
      for (int j = 0; j < filas; j++)
      {
        final Celda celda = celdas.get(numeroDeCelda);
        celda.GetNumeroDeEstaCelda(numeroDeCelda);

        celda.vertice1[x] = (ancho/filas) * j + _posX;
        celda.vertice1[y] = (alto/columnas) * i + alturaInicial + _posY;
        celda.SetSize();
        if (mostrar) celda.Dibujar();
        numeroDeCelda++;
      }

    if (numeroDeCelda == filas * columnas) numeroDeCelda = 0;
  }


  //--------------------------------- 
  void OnCollisionCeldas()
  {
    for (Celda _celdas : celdas)
      _celdas.OnCollision();
  }
  

  //--------------------------------- 
  void Interfaz()
  {
    if (mostrar)
      for (Celda _celdas : celdas) {
        _celdas.IndicadorCeldaOcupada();
        _celdas.IndicarNumeroDeCelda();
      }
  }
  
  
  //--------------------------------- 
  void ReiniciarPreSeleccion()
  {
    for (Celda _celdas : celdas) 
      _celdas.preSeleccionada = false;
  }
}





//===================================================== CLASE CELDA =====================================================
class Celda
{
  float[] vertice1 = new float [2];//Obtienen valor en "Tabla"
  float[] size = new float [2];

  boolean ocupada;
  boolean preSeleccionada;
  int numeroDeEstaCelda;


  //---------------------------------
  void SetSize()
  {
    size[x] = width/tabla.filas;
    size[y] = tabla.alto/tabla.columnas;
  }


  //---------------------------------
  void Dibujar()
  {
    pushStyle();
    noFill();
    strokeWeight(2);
    stroke(0);
    rect(vertice1[x], vertice1[y], size[x], size[y]);
    popStyle();
  }


  //---------------------------------
  void GetNumeroDeEstaCelda(int _numeroDeCelda)
  {
    numeroDeEstaCelda = _numeroDeCelda;
  }


  //---------------------------------
  void OnCollision()
  {

    Forma forma;

    if (formas.size() > 0)
    {
      forma = formas.get(formas.size()-1);
      for (Rectangulo _rect : forma.rectangulos)
        if (vertice1[x] == _rect.vertice1[x]  &&  vertice1[y] == _rect.vertice1[y])
        {
          if (forma.isPreview == false) ocupada = true;
          else preSeleccionada = true;
        }
    }

    if (blocNotas.miniForma.size() > 0)
    {
      forma = blocNotas.miniForma.get(0);
      for (Rectangulo _rect : forma.rectangulos)
        if (vertice1[x] == _rect.vertice1[x]  &&  vertice1[y] == _rect.vertice1[y])
        {
          ocupada = true;
        }
    }
  }


  //---------------------------------
  void IndicadorCeldaOcupada() 
  {
    pushStyle();
    if (!ocupada  &&  !preSeleccionada) fill(color(135, 100, 100));
    else if (preSeleccionada  &&  !ocupada) fill(color(45, 100, 100));
    else if (ocupada) fill(#a8323e);
    circle((vertice1[x] + size[x]) - 10, vertice1[y] + 10, 10);
    popStyle();
  }

  void IndicarNumeroDeCelda()
  {
    pushStyle();
    textSize(12);
    fill(#000000);
    text(numeroDeEstaCelda, vertice1[x] + (size[x]/2), vertice1[y] + (size[y]/2));
    popStyle();
  }
}
