//Para poder ejecutar el juego es necesario tener todas las librerias instaladas
//TUIO es parte de Reactivision y se debe pegar manualmente en la carpeta de librerias de proccessing. Se puede conseguir aqui: https://www.tuio.org/?software
//Minin se puede instalar desde processing.
//Tambien se puede ejecutar el exportado aunque el rendimiento no es el mejor.

//Si no se juega con el reconocimiento visual, puede jugarse con las flechas.

import TUIO.*;
import fisica.*;
import ddf.minim.*;

FWorld mundo;
Minim minim
TuioProcessing tuio;

Camara camara;
Intro intro;
Mapa mapa;
Bird bird;
AudioPlayer musica;

int x = 0, y = 1;
float transX, transY;
PImage fondo;

//------------------------ DEFINIR EL TAMAÑO DE LA UNIDAD
float unidad = 40;
String escena = "intro";
boolean desarrollador = false;
boolean modoAsistido = true;




void setup() {
  //P2D cambia el motor a OpenGL que es mas eficiente con graficos
  size(1280, 720, P2D);
  //frameRate(30);
  smooth();

  Fisica.init(this);
  minim = new Minim(this);
//tuio = new TuioProcessing(this);

  mundo = new FWorld(-2000, -2000, 4000, 3000);
  mundo.setGravity(0, 0);

  camara = new Camara();
  intro = new Intro();
  bird = new Bird();
  mapa = new Mapa();

  musica = minim.loadFile(dataPath("sonidos") + "//" + "Hidden Temple.mp3", 2048);
  musica.loop();
}



void draw() {

  if (escena == "intro") {
    intro.RunLoaderScreen();
  }

  if (escena == "menu") {
    intro.RunMenu();
  }


  if (escena == "juego"  ||  escena == "ganaste") 
  {
    background(#b1adff);
    image( fondo, -750-bird.getX(), -2000-bird.getY() );

    pushMatrix();
    transX = (width/2)-bird.getX();
    transY = 400-bird.getY();
    translate(transX, transY);
    mundo.draw();
    mundo.step();

    bird.Update();
    mapa.DibujarNiveles();
    popMatrix();

    if (desarrollador) {
      camara.grilla.Update();
      //println("frameRate " + frameRate);
    }

    if (escena == "ganaste") {
      new PantallaDeVictoria();
      bird.estado = "parado";
      noLoop();
    }
  }//end escena==juego
}//end Draw


//--------------------------------------------
void contactStarted(FContact colision) {

  FBody other = null;
  if (colision.getBody1() == bird) {
    other = colision.getBody2();
  } else if (colision.getBody2() == bird) {
    other = colision.getBody1();
  }

  if (other == null) {
    return;//Si no hay colision, return.
  }

  if (other.getName() == "reset") {
    bird.ColisionResetArea(other);
  }

  if (other.getName() == "areaDeVuelo") {
    bird.ColisionAreaDeVuelo(other);
  }

  if (other.getName() == "areaNoElegida") {//Area interactiva
    bird.ColisionAreaInteractivaDePlataformas(other);
  }

  if (other.getName() == "areaDeCaida") {
    bird.ColisionAreaDeCaida(other);
  }

  if (other.getName() == "checkPoint") {
    bird.ColisionCheckPoint(other);
  }

  if (other.getName() == "plataforma") {
    bird.ColisionPlataforma(colision, other);
    //println(other.getName() + frameCount);
  }

  if (other.getName() == "cambiarDireccion_derecha"  ||  other.getName() == "cambiarDireccion_izquierda") {
    bird.ColisionAreaCambiarDireccion(other);
  }

  //println(colision.getNormalY());
  //println(other.getName());
  //println(bird.estado);
}//end contactStarted
