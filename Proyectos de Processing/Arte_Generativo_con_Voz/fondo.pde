class Fondo {
  PImage[] imagenFondo = new PImage[3];
  color colorTrazo;
  color colorFondo;
  color colorLienzo;
  int cualFondo;

  private Circulo circulo;



  //---------------------------------
  Fondo() {
    circulo = new Circulo();
    for (int i=0; i < 3; i++) 
      imagenFondo[i] = loadImage("fondo" + i + ".png");


    colorTrazo = color(random(200, 230), 100, random(40, 75));
    colorLienzo = color(random(0, 24), random(0, 6), random (88, 100));
    cualFondo = int (random(imagenFondo.length));

    int randomColorFondo = int(random(imagenFondo.length));
    if (randomColorFondo == 0) colorFondo = #d52780;
    if (randomColorFondo == 1) colorFondo = #f063a3;
    if (randomColorFondo == 2) colorFondo = #f09acb;
  }


  //---------------------------------
  void CrearFondo() {
    background(colorLienzo);
    circulo.dibujar();
    
    pushStyle();
    tint(colorFondo);
    image(imagenFondo[cualFondo], 0, -80);
    popStyle();
  }
}




//===================================================== CLASE CIRCULO =====================================================
class Circulo {

  PImage[] imagen = new PImage[2];
  float positionX, positionY;
  float size;

  color colorCirculo;
  int i = int(random(0, 2));


  Circulo() {
    imagen[0] = loadImage("circulo0.png");
    imagen[1] = loadImage("circulo1.png");
    colorCirculo = color(random(245, 300), 75, 75);

    positionX = random(150, width-300);
    positionY = random((height/2) + 280, (height/2) + 400);
    size = int(random(150, 180));
  }


  //---------------------------------
  void dibujar() {
    pushStyle();
    imageMode(CENTER);
    tint(colorCirculo);
    image(imagen[i], positionX, positionY, size, size);
    popStyle();
  }
}
