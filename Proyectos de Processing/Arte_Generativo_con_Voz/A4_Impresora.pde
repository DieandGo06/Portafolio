class Impresora {
  private Forma forma;



  //--------------------------------- 
  void CrearFormaDefinitiva() 
  {
    if (audioSeleccion.seleccionSize > 0  &&  amp < umbralRuido  &&  !mousePressed)
    {
      audioSeleccion.GetFormaDeLaSeleccion();
      distribucion.EliminarUltimaForma();
      distribucion.CrearNuevaForma();
      OcultarCelda(distribucion.celda);

      ReferenciarForma();
      DibujarForma();
      DibujarTrazo();
      tabla.OnCollisionCeldas();//Marca como 'ocupadas' las celdas.

      ReiniciarSistemas();
      distribucion.ProbarFormaInvisible(true);
    }
  }



  //--------------------------------- 
  void DibujarForma()
  { 
    int size1 = audioSeleccion.celdasPrimeraLinea;
    int size2 = audioSeleccion.celdasSegundaLinea;

    String formaTipo = audioSeleccion.formaTipo;
    if (formaTipo == "L-normal") forma.LNormal(size1, size2);
    if (formaTipo == "L-acostada") forma.LAcostada(size1, size2);
    if (formaTipo == "L-invertida")forma.LInvertida(size1, size2);
    if (formaTipo == "horizontal") forma.RectaHorizontal(size1);
    if (formaTipo == "vertical") forma.RectaVertical(size1);
  }


  //--------------------------------- 
  void DibujarTrazo()
  {
    if (forma.isPreview)
      if (!forma.OnCollision(tabla)) {
        forma.isPreview = false;
        forma.CrearTrazo();
        forma.DibujarTrazo();
      }
  }


  //--------------------------------- 
  void CambiarFormaConSonido() {
    if (audioSeleccion.cooldownCambiarForma < 0)
      if (audioSeleccion.segundos > 0.1  &&  audioSeleccion.segundos < 0.5)
        if (amp < umbralRuido) 
        {
          CambiarForma();
          audioSeleccion.ReiniciarSeleccion();
        }
  }


  //--------------------------------- 
  void ReiniciarSistemas()
  {
    distribucion.ReiniciarParametros();
    tabla.ReiniciarPreSeleccion();

    blocNotas.EliminarMiniForma();
    blocNotas.ReiniciarParametros();
    blocNotas.ReiniciarMiniTabla();

    audioSeleccion.ReiniciarSeleccion();
  }

  //--------------------------------- 
  void CambiarForma()
  {
    ReiniciarSistemas();
    distribucion.EliminarUltimaForma();
    distribucion.ProbarFormaInvisible(true);
  }

  //--------------------------------- 
  void OcultarCelda(Celda _celda)
  {
    pushStyle();
    noFill();
    stroke(fondo.colorFondo);
    rect(_celda.vertice1[x], _celda.vertice1[y], _celda.size[x], _celda.size[y]);
    popStyle();
  }



  //--------------------------------- 
  void ReferenciarForma()
  {
    forma = distribucion.forma;
  }
}
