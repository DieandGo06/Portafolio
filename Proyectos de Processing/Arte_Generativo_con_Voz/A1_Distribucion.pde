class Distribucion {
  private Celda celda;
  private Forma forma;
  int numeroCelda;
  int formaSize;

  String formaCreada;
  int reintentos;
  int formaSizeAnterior;


  //--------------------------------- 
  void ProbarFormaInvisible(boolean unaSolaEjecucion)
  {
    if (unaSolaEjecucion) 
    {
      formaSize = int(random(1, 8));
      EvitarRepeticionFormaSize();

      SeleccionarCelda();
      CrearNuevaForma();
      ProbarForma();
      unaSolaEjecucion = false;
    }
  }


  //--------------------------------- 
  void SeleccionarCelda()
  {
    //numeroCelda++;
    celda = tabla.celdas.get(numeroCelda);
    if (celda.ocupada) {
      numeroCelda++;
      celda = tabla.celdas.get(numeroCelda);
    }
  }


  //--------------------------------- 
  void CrearNuevaForma() {
    formas.add(new Forma(celda.vertice1[x], celda.vertice1[y]));
    forma = formas.get(formas.size()-1);
    formaCreada = "Forma " + (formas.size()-1) + " creada en celda " + celda.numeroDeEstaCelda;
  }


  //--------------------------------- 
  void ProbarForma()
  { 
    if (forma.isPreview)
      for (int i=0; i < 7; i++)
      {
        SeleccionarForma(forma);

        if (forma.OnCollision(tabla)) {
          EliminarUltimaForma();
          CrearNuevaForma();
        } else {//Se queda con la forma que probo.
          DibujarCeldaSeleccionada();
          println(formaCreada);
          reintentos = 0;
          formaSizeAnterior = formaSize;
          break;
        }

        if (i == 6) {
          //ReintentarOtraForma();
          //celda.ocupada = true;
          ReintentarOtraForma();
          //CambiarCeldaSeleccionada(true);
          //i = 0;
        }
      }
  }


  //--------------------------------- 
  void SeleccionarForma(Forma _forma)
  {
    //println("numero de intento.............. " + intentos);
    int intentos;
    switch (formaSize) {
    case 1: 
      intentos = int(random(0, 8));
      if (intentos==0) _forma.RectaVertical(1);
      if (intentos==1) _forma.RectaHorizontal(1);
      if (intentos>=2) ReintentarOtraForma();//75%
      break;

    case 2:
      intentos = int(random(0,4));
      if (intentos==0) _forma.RectaVertical(2);
      if (intentos==1) _forma.RectaHorizontal(2);
      if (intentos>=2) ReintentarOtraForma();//50%
      break;

    case 3:
      intentos = int(random(0, 4));
      if (intentos==0) _forma.RectaVertical(3);
      if (intentos==1) _forma.RectaHorizontal(3);
      if (intentos>=2) ReintentarOtraForma();//50%
      break;

    case 4:
      intentos = int(random(0, 5));
      if (intentos==0) _forma.LNormal(2, 2);
      if (intentos==1) _forma.LAcostada(2, 2); 
      if (intentos==2) _forma.LInvertida(2, 2);
      if (intentos==3) _forma.RectaVertical(4);
      if (intentos>=4) ReintentarOtraForma();//20%
      break;

    case 5:
      intentos = int(random(0, 9));
      if (intentos==0) _forma.LNormal(2, 3);
      if (intentos==1) _forma.LNormal(3, 2);
      if (intentos==2) _forma.LAcostada(2, 3);
      if (intentos==3) _forma.LAcostada(3, 2);
      if (intentos==4) _forma.LInvertida(2, 3);
      if (intentos==5) _forma.LInvertida(3, 2);
      if (intentos>=6) ReintentarOtraForma();//33%
      break;

    case 6:
      intentos = int(random(0, 8));
      if (intentos==0) _forma.LNormal(3, 3);
      if (intentos==1) _forma.LNormal(4, 2);
      if (intentos==2) _forma.LAcostada(2, 4);
      if (intentos==3) _forma.LAcostada(3, 3);
      if (intentos==4) _forma.LInvertida(3, 3);
      if (intentos==5) _forma.LInvertida(4, 2);
      if (intentos>=6) ReintentarOtraForma();//26%
      break;

    case 7:
      intentos = int(random(0, 3));
      if (intentos==0) _forma.LNormal(4, 3);
      if (intentos==1) _forma.LInvertida(4, 3);
      if (intentos>=2) ReintentarOtraForma();//33%
      break;
    }
  }


  //--------------------------------- 
  void ReintentarOtraForma() {

    reintentos++;
    println("  Reintento " + reintentos + " con otra tamaño");

    if (reintentos < 8) 
    {
      EliminarUltimaForma();
      ProbarFormaInvisible(true);
    } 
    if (reintentos >= 8  &&  reintentos < 10)
    {
      EliminarUltimaForma();
      formaSize = 2;
      SeleccionarCelda();
      CrearNuevaForma();
      ProbarForma();
    }
    if (reintentos >= 10) {
      EliminarUltimaForma();
      formaSize = 1;
      SeleccionarCelda();
      CrearNuevaForma();
      ProbarForma();
      reintentos = 0;
    }
  }




  //--------------------------------- 
  void BloquearCeldasInutiles()
  {
    for (int i=130; i < 140; i++) {
      tabla.celdas.get(i).ocupada = true;
    }
  }



  //--------------------------------- 
  void EliminarUltimaForma() {
    //println("FORMA " + (formas.size()-1) + " ELIMINADA");
    formas.remove(formas.size()-1);
  }

  //--------------------------------- 
  void ReiniciarParametros() {
    forma = null;
    formaSize = 0;
  }

  //--------------------------------- 
  void EvitarRepeticionFormaSize() {
    for (int i=0; i < 5; i++) 
      if (formaSize == formaSizeAnterior) 
        formaSize = int(random(3, 8));
  }

  //--------------------------------- 
  void DibujarCeldaSeleccionada()
  {
    pushStyle();
    noFill();
    stroke(color(135, 100, 100));
    rect(celda.vertice1[x], celda.vertice1[y], celda.size[x], celda.size[y]);
    popStyle();
  }
}  
