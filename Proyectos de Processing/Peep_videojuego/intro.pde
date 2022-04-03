class Intro {

  PImage intro, titulo, globoDialogo1, globoDialogo2, globoDialogo3;
  boolean fondoLoadState;
  PImage[] loadAnimation = new PImage[8];

  int fadeText = 255;
  int tiempoEnMenu;
  boolean isFadingText;


  Intro() {
    //Imagen de fondo pesada
    fondo = requestImage("background.jpg");

    //Cargar Imagenes de loadScreen
    intro = loadImage("intro.jpg");
    for (int i=0; i<loadAnimation.length; i++) {
      loadAnimation[i] = loadImage(dataPath("girar-derecha") + "//" + "birdBall0" + i + "Der.png");
    }

    //Cargar Imagenes de Menu
    titulo = loadImage("nombre.png");
    globoDialogo1 = loadImage("globoDialogo-1.png");
    globoDialogo2 = loadImage("globoDialogo-2.png");
    globoDialogo3 = loadImage("globoDialogo-3.png");
  }


  void RunLoaderScreen() {
    image(intro, 0, 0);
    int cambiarSprite = frameCount/6;
    image(loadAnimation[cambiarSprite%loadAnimation.length], width-100, height-85);

    println("fondo cargado: " + isFondoCargado());
    if (isFondoCargado()) escena = "menu";
  }

  void RunMenu() {
    image(intro, 0, 0);

    pushStyle();
    textSize(24);
    textAlign(CENTER);
    fill(230, fadeText);
    text("presiona espacio o muestra el patron", width/2, height-50);
    imageMode(CENTER);
    image(titulo, width/2, (height/2) - 200);
    popStyle();

    if (fadeText <= 50) {
      isFadingText = false;
    } else if (fadeText >= 255) {
      isFadingText = true;
    }
    float speedFade = 2;
    if (isFadingText) {
      fadeText -= speedFade;
    } else fadeText += speedFade;


    if (tiempoEnMenu > 60 && tiempoEnMenu < 300) {
      //image de texto 1
      image(globoDialogo1, width/2+40, (height/2)-125);
    }
    if (tiempoEnMenu > 300 && tiempoEnMenu < 540) {
      //image de texto 2
      image(globoDialogo2, width/2-75, (height/2)-160);
    }
    if (tiempoEnMenu > 540 && tiempoEnMenu < 780) {
      //image de texto 3
      image(globoDialogo3, width/2, (height/2)-155);
    }
    tiempoEnMenu++;


    //Cambiar Escena
    if (keyPressed) {
      if (key == ' ')
        escena = "juego";
    }
  }


  boolean isFondoCargado() {
    if ( (fondo.width != 0) && (fondo.width != -1) ) {
      return true;
    }
    return false;
  }
}//end class
