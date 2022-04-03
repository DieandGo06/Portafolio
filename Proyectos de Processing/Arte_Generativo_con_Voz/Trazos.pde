class Trazo {

  private Forma forma;
  PImage[] img = new PImage[2];
  
  int indexTrazo;
  String extensionPNG;
  int[] figuraSize = new int[2];

  
  Trazo (Forma perteneceAForma) {
    forma = perteneceAForma;
  }
  

  void Display()
  {
    if (forma.tipo != null)
    {
      CargarImagenes();
      Dibujar(img[0], img[1]);
    }
  }


  //---------------------------------
  void Dibujar(PImage cualTrazo, PImage manchaBlanca) 
  {
    cualTrazo.filter(INVERT);
    cualTrazo.mask(cualTrazo);
    manchaBlanca.filter(INVERT);
    manchaBlanca.mask(manchaBlanca);

    pushStyle();
    tint(fondo.colorTrazo);
    image(cualTrazo, forma.position[x] - CorreccionEnX(), forma.position[y]);
    popStyle();

    pushStyle();
    color manchaColor = color(230, random(4, 10), random(95, 100), 245);
    tint(manchaColor);
    image(manchaBlanca, forma.position[x] - CorreccionEnX(), forma.position[y]);
    popStyle();
  }


  //---------------------------------
  float CorreccionEnX() {
    Celda celda = distribucion.celda;
    if (forma.tipo == "L-invertida")
      return celda.size[x] * (figuraSize[1] - 1);
    return 0;
  }


  //---------------------------------
  void CargarImagenes()
  {
    String nombre = forma.tipo + figuraSize[0] + "x" + figuraSize[1];
    String extension = ".png";

    for (int i=0; i < 2; i++)
    {
      if (i == 1) extension = "-(blanco).png";
      img[i] = loadImage(nombre + extension);
    }
  }


  //---------------------------------
  void GetFiguraSize(int _figura1Size, int _figura2Size)
  {
    figuraSize[0] = _figura1Size;
    figuraSize[1] = _figura2Size;
  }

  
}








//===================================================== CLASE SPRAY =====================================================
class Spray {
  PImage[] spray = new PImage[10];
  float randomH = random(210, 360);
  color randomColor = color(randomH, random(60, 80), random(35, 70));


  //---------------------------------
  void CargarImagenes() {
    spray[0] = loadImage("spray(1).png"); 
    spray[1] = loadImage("spray(2).png"); 
    spray[2] = loadImage("spray(3).png");
    spray[3] = loadImage("spray(4).png"); 
    spray[4] = loadImage("spray(5).png");
    spray[5] = loadImage("spray(6).png");
    spray[6] = loadImage("spray(7).png");
    spray[7] = loadImage("spray(8).png");
  }

  //---------------------------------
  void Dibujar(int numCelda, int numSpray) 
  {
    Celda celda = tabla.celdas.get(numCelda);
    float size = 175 * random(0.75, 1.25);
    CargarImagenes();

    pushStyle();
    imageMode(CENTER);
    tint(randomColor);
    image(spray[numSpray], celda.vertice1[x], celda.vertice1[y], size, size);
    popStyle();
  }

  //---------------------------------
  void DibujarTodos()
  {
    for (int i = 0; i < 4; i++)
      for (int j = 0; j < 8; j++)
      {
        int celda;
        if (i == 0) celda = int(random(0, 10));
        else if (i == 1) celda = int(random(30, 60));
        else if (i == 2) celda = int(random(60, 90));
        else celda = int(random(90, 120));

        Dibujar(celda, j);
      }
  }


  //---------------------------------
  void DibujarEnCirculo()
  {
    int i = int(random(0, 2));
    if (i==0) spray[8] = loadImage("spray(9).png");
    if (i==1) spray[8] = loadImage("spray(10).png");

    float tono = randomH - 80;
    float size = fondo.circulo.size + 15;
    float [] position = { fondo.circulo.positionX, fondo.circulo.positionY };

    pushStyle();
    imageMode(CENTER);
    tint(color(tono, random(70, 90), random(60, 90), 230));
    image(spray[8], position[x], position[y], size, size);
    popStyle();
  }
}
