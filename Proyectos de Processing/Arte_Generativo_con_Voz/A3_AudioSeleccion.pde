class Audio {
  float time, segundos;
  float cooldownCambiarForma;

  int seleccionSize;
  int celdasPrimeraLinea;
  int celdasSegundaLinea;
  String formaTipo;



  //--------------------------------- 
  //Indica que celda se pintara. 
  //El sistema guarda las celdas ocupadas de la miniTabla de forma ordenada.
  //Con las L-invertidas ese orden no funciona ya que la linea horizontal se guarda a la inversa.
  //Por ello, si la forma es una L-invertida y ya se seleccionaron la celdas verticales, se realizan calculos para corregir el orden.
  //... Si no pasa nada de esto... la celda selenccionada es 'i'.

  void SeleccionarCeldas()
  {
    for (int i=0; i < TiempoToCeldas(); i++)
    {
      Celda celda;
      Forma miniForma = blocNotas.miniForma.get(0);
      // Guarda las celdas seleccionadas en dos variables usadas para dibujar la forma y trazo final
      if (i < miniForma.linea1_size) celdasPrimeraLinea = i+1;
      if (i >= miniForma.linea1_size) celdasSegundaLinea = (i+1) - (celdasPrimeraLinea-1);

      //Explicacion al final
      if (miniForma.tipo == "L-invertida"  &&  i >= miniForma.linea1_size-1) {
        celda = blocNotas.celdasOcupadas.get(CalcularIndexSegundaLinea_LInvertida(i));
      } else {
        celda = blocNotas.celdasOcupadas.get(i);
      }
      PintarSeleccion(celda);
    }
    //println("celdas P... " + celdasPrimeraLinea);
    //println("celdas S... " + celdasSegundaLinea);
  }


  //--------------------------------- 
  int CalcularIndexSegundaLinea_LInvertida(int indexFor) {
    Forma miniForma = blocNotas.miniForma.get(0);
    int a = blocNotas.celdasOcupadas.size()-1; 
    int b = indexFor - (miniForma.linea1_size -1);
    return a - b;
  }



  //---------------------------------
  int TiempoToCeldas()
  {
    if (segundos >= 0  &&  segundos < 0.5) seleccionSize = 0;
    else if (segundos >= 0.5  &&  segundos < 1.0) seleccionSize = 1;
    else if (segundos >= 1.0  &&  segundos < 1.5) seleccionSize = 2;
    else if (segundos >= 1.5  &&  segundos < 2.0) seleccionSize = 3;
    else if (segundos >= 2.0  &&  segundos < 2.5) seleccionSize = 4;
    else if (segundos >= 2.5  &&  segundos < 3.0) seleccionSize = 5;
    else if (segundos >= 3.0  &&  segundos < 3.5) seleccionSize = 6;

    if (seleccionSize >=  blocNotas.celdasOcupadas.size()) 
      seleccionSize = blocNotas.celdasOcupadas.size();

    return seleccionSize;
  }


  //--------------------------------- 
  void GetFormaDeLaSeleccion()
  {
    Forma miniForma = blocNotas.miniForma.get(0);

    if (celdasPrimeraLinea <= miniForma.linea1_size  &&  celdasSegundaLinea == 0 ) 
    {
      if (miniForma.isLinea1Vertical) {
        formaTipo = "vertical";
      } else formaTipo = "horizontal";
    } else formaTipo = miniForma.tipo;
  }



  //--------------------------------- 
  void PintarSeleccion (Celda _celda)
  {
    pushStyle();
    noFill();
    strokeWeight(2);
    stroke(color(45, 100, 100));
    rect(_celda.vertice1[x], _celda.vertice1[y], _celda.size[x], _celda.size[y]);
    popStyle();
  }



  //--------------------------------- 
  void Cronometro()
  {
    cooldownCambiarForma--;
    if (amp > umbralRuido  ||  mousePressed) {
      time++;
    } 
    segundos = time/60;
  }


  //--------------------------------- 
  void ReiniciarSeleccion() {
    time = 0;
    celdasPrimeraLinea = 0; 
    celdasSegundaLinea = 0;
    cooldownCambiarForma = 0.5*60;
  }


  //--------------------------------- 
  void ReiniciarCronometro() {
    if (amp < umbralRuido & !mousePressed) time = 0;
  }


  //--------------------------------- 
  void InterfazCronometro() {
    pushStyle();
    fill(#FFFFFF);
    rect(15, 575, 50, 50);
    fill(0);
    textSize(16);
    text(time/60, 15, 600);
    popStyle();
  }
}
