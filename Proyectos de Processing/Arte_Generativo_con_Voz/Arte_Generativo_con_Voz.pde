//============================
//Si no se tiene la libreria ni programa de audio, se debe comentar la linea 6, 7 y el metodo void oscEvent(OscMessage m).
//Se podra interactuar con el programa presionando el click izquierdo del mouse. Entre mas tiempo presionado, mas grande sera la seleccion.
//Link del programa de audio https://puredata.info/downloads/osc

import oscP5.*;
OscP5 osc;
GestorSenial gestorAmp;
float amp;
float umbralRuido = 50;
//============================

Fondo fondo;
Tabla tabla;
Spray spray;

Distribucion distribucion;
BlocNotas blocNotas;
Impresora impresora;
Audio audioSeleccion;

ArrayList<Forma> formas;
int x = 0, y = 1;



void setup()
{
  size(800, 800);
  imageMode(CORNER);
  colorMode(HSB, 360, 100, 100);

  //Gestor de Sonido
  //osc = new OscP5(this, 12345);
  gestorAmp = new GestorSenial( umbralRuido, 100, 0.9 );


  //Inicializacion de Variables 
  fondo = new Fondo();
  tabla = new Tabla();
  spray = new Spray();
  audioSeleccion = new Audio();
  formas = new ArrayList<Forma>();


  //Metodos iniciales
  fondo.CrearFondo();
  tabla.Crear(0, 0);
  tabla.mostrar = false;//====> Activa la visualizacion de la tabla.


  //1... Sistema de Distribucion
  distribucion = new Distribucion();
  distribucion.BloquearCeldasInutiles();
  distribucion.ProbarFormaInvisible(true);

  //2... Bloc de Notas
  blocNotas = new BlocNotas();
  blocNotas.DibujarInterfaz();
  blocNotas.CrearMiniTabla();
  blocNotas.ReiniciarMiniTabla();//Dibuja la tabla

  //3... Sistema de Interactividad 
  //---

  //4... Impresora
  impresora = new Impresora();
} 






//------------------------------------------
void draw()
{
  tabla.Crear(0, 0);
  tabla.OnCollisionCeldas();
  tabla.Interfaz();

  blocNotas.CrearMiniTabla();
  blocNotas.CrearMiniForma();
  blocNotas.miniTabla.Interfaz();
  blocNotas.miniTabla.OnCollisionCeldas();

  audioSeleccion.Cronometro();
  audioSeleccion.SeleccionarCeldas();
  //audioSeleccion.InterfazCronometro();//====> Muestra el cronometro.

  impresora.CrearFormaDefinitiva();
  impresora.CambiarFormaConSonido();//====> Comentar si causa problemas.
  
  audioSeleccion.ReiniciarCronometro();
  if (distribucion.numeroCelda > 109) {
    DetenerPrograma();
  }
}



//------------------------------------------
void oscEvent(OscMessage m) {
  //datos de amplitud
  if (m.addrPattern().equals("/amp")) {
    amp = m.get(0).floatValue();
    //println("amp:" + amp);//====> Imprime los datos de amp.
  }
}


//------------------------------------------
void DetenerPrograma() {
  distribucion.EliminarUltimaForma();
  impresora.OcultarCelda(distribucion.celda);
  
  tabla.mostrar = false;
  fondo.CrearFondo();

  for (Forma _forma : formas) 
    _forma.DibujarTrazo();

  spray.DibujarTodos();
  spray.DibujarEnCirculo();
  noLoop();
}



//------------------------------------------
void keyPressed()
{ 
  if (key == ' ') {
    distribucion.forma.isPreview = false;
    distribucion.forma.CrearTrazo();
    distribucion.forma.DibujarTrazo();
    tabla.OnCollisionCeldas();

    distribucion.ProbarFormaInvisible(true);
  }

  if (key == 'q') {
    distribucion.numeroCelda = 110;
  }

  if (key == 'e') {
    spray.DibujarTodos();
    spray.DibujarEnCirculo();
    noLoop();
  }

  if (key == 'u') {
    impresora.CambiarForma();
  }
}
