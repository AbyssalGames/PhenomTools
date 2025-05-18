using System;
using System.Collections;
using PhenomTools.Utility;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Validation;
[assembly: RegisterValidator(typeof(TimeSpanParseableValidator))]
[assembly: RegisterValidator(typeof(StringLengthValidator))]
[assembly: RegisterValidator(typeof(CollectionLengthValidator<>))]
[assembly: RegisterValidator(typeof(SpriteSizeValidator))]
[assembly: RegisterValidator(typeof(SpriteReferenceSizeValidator))]
#endif

namespace PhenomTools.Utility
{
  public class ValidTimeSpanAttribute : Attribute {}

  public class StringLengthAttribute : Attribute
  {
    public uint MinLength;
    public uint MaxLength;
    public bool AllowNull;
  }

  public class CollectionLengthAttribute : Attribute
  {
    public uint MinCount = 0;
    public uint MaxCount = uint.MaxValue;
    public bool AllowNull = true;
    public bool AllowNullElements = true;
  }

  public class SpriteSizeAttribute : Attribute
  {
    public uint Length;
    public uint Width;
  }

#if UNITY_EDITOR
  public class TimeSpanParseableValidator : AttributeValidator<ValidTimeSpanAttribute, string>
  {
    protected override void Validate(ValidationResult result)
    {
      if (TimeSpan.TryParse(ValueEntry.SmartValue, out TimeSpan _))
        result.ResultType = ValidationResultType.Valid;
      else
      {
        result.ResultType = ValidationResultType.Error;
        result.Message = "This string is not parseable as a TimeSpan value.";
      }
    }
  }

  public class StringLengthValidator : AttributeValidator<StringLengthAttribute, string>
  {
    protected override void Validate(ValidationResult result)
    {
      string value = ValueEntry.SmartValue;

      if (value == null && !Attribute.AllowNull)
      {
        result.ResultType = ValidationResultType.Error;
        result.Message = $"{ValueEntry.Property.Name} cannot be null.";
      }
      else if (value.Length < Attribute.MinLength)
      {
        result.ResultType = ValidationResultType.Error;
        result.Message = $"{ValueEntry.Property.Name} is too short ({value.Length} chars). Must be at least {Attribute.MinLength} characters long.";
      }
      else if (value.Length > Attribute.MaxLength)
      {
        result.ResultType = ValidationResultType.Error;
        result.Message = $"{ValueEntry.Property.Name} is too long ({value.Length} chars). Must be {Attribute.MaxLength} characters or less.";
      }
      else
        result.ResultType = ValidationResultType.Valid;
    }
  }

  public class CollectionLengthValidator<Tcollection> : AttributeValidator<CollectionLengthAttribute, Tcollection> where Tcollection : IList
  {
    protected override void Validate(ValidationResult result)
    {
      if (ValueEntry.SmartValue == null && !Attribute.AllowNull)
      {
        result.ResultType = ValidationResultType.Error;
        result.Message = $"{ValueEntry.Property.Name} cannot be null.";
      }
      else if (ValueEntry.SmartValue.Count < Attribute.MinCount)
      {
        result.ResultType = ValidationResultType.Error;
        result.Message = $"{ValueEntry.Property.Name} only contains {ValueEntry.SmartValue.Count} items. Must have at least {Attribute.MinCount} item(s).";
      }
      else if (ValueEntry.SmartValue.Count > Attribute.MaxCount)
      {
        result.ResultType = ValidationResultType.Error;
        result.Message = $"{ValueEntry.Property.Name} contains too many ({ValueEntry.SmartValue.Count}) items. Must have no more than {Attribute.MaxCount} item(s).";
      }
      else if (!Attribute.AllowNullElements)
      {
        foreach (var item in ValueEntry.SmartValue)
          if(item == null)
          {
            result.ResultType = ValidationResultType.Error;
            result.Message = $"{ValueEntry.Property.Name} contains one or more null elements and this is not supported. First null element found at index {ValueEntry.SmartValue.IndexOf(item)}.";
            break;
          }
      }
      else
        result.ResultType = ValidationResultType.Valid;
    }
  }

  public class SpriteSizeValidator : AttributeValidator<SpriteSizeAttribute, Sprite>
  {
    protected override void Validate(ValidationResult result)
    {
      Sprite sprite = ValueEntry.SmartValue;
      if (sprite == null)
      {
        // This validator is for validating sprite size. If you care about null, then add the [Required] attribute.
        return;
      }

      if (!IsSpriteSizeValid(Attribute, sprite))
        SetErrorValidationResult(result, ValueEntry.Property, Attribute, sprite);
    }

    public static bool IsSpriteSizeValid(SpriteSizeAttribute attribute, Sprite sprite)
    {
      Vector2 spriteDims = sprite.rect.size;
      return (spriteDims.x == attribute.Length) && (spriteDims.y == attribute.Width);
    }

    public static void SetErrorValidationResult(ValidationResult result, InspectorProperty property, SpriteSizeAttribute attribute, Sprite sprite)
    {
      Vector2 spriteDims = sprite.rect.size;
      result.ResultType = ValidationResultType.Error;
      result.Message = $"{property.Name} is designed to be ({attribute.Length}x{attribute.Width}) but the {sprite.name} sprite is ({spriteDims.x}x{spriteDims.y}).";
    }
  }

  public class SpriteReferenceSizeValidator : AttributeValidator<SpriteSizeAttribute, SpriteAssetReference>
  {
    protected override void Validate(ValidationResult result)
    {
      SpriteAssetReference spriteReference = ValueEntry.SmartValue;
      if((spriteReference == null) || (spriteReference.editorAsset == null))
      {
        // This validator is for validating sprite size. If you care about null, then add the [SharedRequireAddress] and/or [Required] attribute(s).
        return;
      }

      Sprite sprite = spriteReference.editorAsset;
      if (!SpriteSizeValidator.IsSpriteSizeValid(Attribute, sprite))
        SpriteSizeValidator.SetErrorValidationResult(result, ValueEntry.Property, Attribute, sprite);
    }
  }
#endif
}