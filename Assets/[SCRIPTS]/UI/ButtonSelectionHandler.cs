using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, ISelectHandler
{
    public enum ButtonType
    {
        Faldridge,
        Aldrac,
        Minnic,
        Frostholm,
        Delstan,
        Stonekiin,
        EastEderwin,
        Omnia,
        Orz,
        Erinstrad,
        Novaridge,

    }

    [SerializeField] public string descriptionField;
    [SerializeField] Transform mapLocation;

    public ButtonType buttonType;

    public void OnSelect(BaseEventData eventData)
    {
        // Perform actions based on the button type
        switch (buttonType)
        {
            case ButtonType.Faldridge:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();

                break;
            case ButtonType.Aldrac:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();
                break;
            case ButtonType.Minnic:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();
                break;
            case ButtonType.Frostholm:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();
                break;
            case ButtonType.Delstan:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();
                break;
            case ButtonType.Stonekiin:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();
                break;
            case ButtonType.EastEderwin:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();
                break;
            case ButtonType.Omnia:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();
                break;
            case ButtonType.Orz:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();
                break;
            case ButtonType.Erinstrad:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();
                break;
            case ButtonType.Novaridge:
                FillDescriptionText(descriptionField);
                MoveMapMarker(mapLocation);
                AudioManager.instance.UIMove();
                break;
            default:
                break;
        }
    }

    public void FillDescriptionText(string text)
    {
        LevelSelectManager.instance.UpdateDescription(text);
    }

    public void MoveMapMarker(Transform newLocation)
    {
        LevelSelectManager.instance.UpdateMapMarker(newLocation);
    }
}

