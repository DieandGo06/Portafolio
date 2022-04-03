class BlocNotas {
  private Tabla miniTabla;
  private ArrayList<Celda> celdasOcupadas;
  private ArrayList<Forma> miniForma;
  PImage sombra;
  PFont segoe;



  BlocNotas()
  {
    sombra = loadImage("sombra.png");
    segoe = loadFont("SegoeUI-Bold-20.vlw");
    miniTabla = new Tabla(3, 4, 80*3, 42*4);

    celdasOcupadas = new ArrayList<Celda>();
    miniForma = new ArrayList<Forma>();
  }


  //--------------------------------- 
  void CrearMiniForma()
  {
    float posX, posY;
    if (distribucion.forma != null)
    {
      if (distribucion.forma.tipo == "L-invertida") {
        posX = miniTabla.celdas.get(2).vertice1[x];
        posY = miniTabla.celdas.get(2).vertice1[y];
      } else {
        posX = miniTabla.celdas.get(0).vertice1[x];
        posY = miniTabla.celdas.get(0).vertice1[y];
      }

      if (miniForma.size() == 0) {
        miniForma.add(new Forma(posX, posY));
        miniForma.get(0).isPreview = false;
        DibujarMiniForma();
      } else AlmacenarCeldasOcupadas();
    }
  }

  //-------------
  void DibujarMiniForma()
  {
    Forma forma = miniForma.get(0);
    int size1 = distribucion.forma.linea1_size;
    int size2 = distribucion.forma.linea2_size;

    String formaTipo = distribucion.forma.tipo;
    if (formaTipo == "L-normal") forma.LNormal(size1, size2);
    if (formaTipo == "L-acostada") forma.LAcostada(size1, size2);
    if (formaTipo == "L-invertida") forma.LInvertida(size1, size2);
    if (formaTipo == "horizontal") forma.RectaHorizontal(size1);
    if (formaTipo == "vertical") forma.RectaVertical(size1);
  }


  //--------------------------------- 
  void AlmacenarCeldasOcupadas() 
  {
    if (celdasOcupadas.size() == 0)
    {
      for (Celda _celda : miniTabla.celdas)
        if (_celda.ocupada) {
          celdasOcupadas.add(_celda);
          PintarCelda(_celda, fondo.circulo.colorCirculo);
        }
      //println(celdasOcupadas.size());
    }
  }



  //-------------
  void ReiniciarMiniTabla() {
    for (Celda _celda : miniTabla.celdas) {
      _celda.ocupada = false;
      PintarCelda(_celda, #FFFFFF);
    }
  }
  //-------------
  void ReiniciarParametros() {
    celdasOcupadas.clear();
  }
  //-------------
  void EliminarMiniForma() {
    miniForma.remove(miniForma.size()-1);
  }
  //-------------
  void PintarCelda(Celda _celda, color _color) {
    pushStyle();
    strokeWeight(2);
    fill(_color);
    rect(_celda.vertice1[x], _celda.vertice1[y], _celda.size[x], _celda.size[y]);
    popStyle();
  }






  //--------------------------------- 
  void DibujarInterfaz()
  {
    int[] pos = { 500, 500 };
    pushStyle();
    noStroke();

    //Sombra 
    fill(color(0, 0, 0, 32));
    //rect(pos[x]-4, pos[y]-4, 300+4, 300+4);

    //Rectangulo Base
    fill(#FFFFFF);
    image(sombra, pos[x]-4, pos[y]-6);
    rect(pos[x], pos[y], 300, 300);

    //Cabecera
    fill(fondo.circulo.colorCirculo);
    rect(pos[x], pos[y], 300, 50);

    //Texto
    fill(#FFFFFF);
    textFont(segoe);
    textSize(20);
    text("Deci 'U' para cambiar trazo", pos[x]+20, pos[y]+32);

    popStyle();
  }

  //--------------------------------- 
  void CrearMiniTabla()
  {
    pushStyle();
    noFill();
    strokeWeight(2);
    miniTabla.mostrar = false;
    miniTabla.Crear(530, 590);
    textSize(12);
    popStyle();
  }
}
