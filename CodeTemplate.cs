// Project: <Name Of Project>
/// \file <NameOfFile>.<Extension>
/// \copyright (C) year, FG!P. All rights reserved.
/// \date <year>-<month>-<day>
/// \author <Author 1>
/// \author <Author 2>
/// \author <Author n>
/// <summary> 
/// <Description>
/// </summary>

class ClassName //Sustantivo
{

	#region Enums
      #endregion

      #region Delegates
      #endregion

      #region Constants
      #endregion

      #region Variables
      #endregion

      #region Properties
      #endregion

      #region Constructors
      #endregion

      #region Methods
      #endregion 


	
	#region Enums

      //PascalCase
      public enum AnimationSpriteType    
      {
        F2USprite,
        F2UBatchedSprite,
        F2USpriteRenderer,
      }

      #endregion

      #region Delegates

      //Posfijo la palabra Delegate
      //Se recomienda poner como parámetro cual objeto es el que ejecuta el “Delegate.”
      //Se recomienda declarar el “Delegate” como privado y poner un “Property”, para poder tener acceso a ese “Delegate.”
      public delegate void OnButtonClickDelegate(FPLUIButton sender);

      #endregion

      #region Constants

      public const string CONSTANT_NAME = "Something";

      #endregion

      #region Variables

      public int customerCount = 0;
      public int index = 0;
      public string temporaryDescription = "";
      public long distance = (long)velocity * time; 
      private int _initialWidth = 0; 
      protected string  _welcomeMessage = "Hello" ;
      [SerializeField] 
      private TextAsset _textureAtlasBinary; 

      #endregion

      #region Properties

      public TextAsset  TextureAtlasBinary 
      {
            get 
            { 
                return _textureAtlasBinary;  
            }
      }

      #endregion

      #region Constructors
      #endregion

      #region Methods 

      //Inicia con verbo, no utilizar this
      public void InitializePath();
      public void GetPath();

      /// (Monodevelop creates this comment structure by typing /// before any method or structure).
      /// <summary>
      /// Selects the next treasure. 
      /// </summary>
      /// <returns>
      /// The next treasure.
      /// </returns>
      /// \deprecated NewSelectNextTreasure should be used instead.
      /// \pre The ‘Treasures’ structure must be initialized beforehand.
      /// \warning Loops back to first treasure if last treasure is reached.
      public int GetDimension(GameObject normalField); //Params camelCase

      #endregion 
}

public interface IComponent //Comienzan con I, luego PascalCase
{

}



