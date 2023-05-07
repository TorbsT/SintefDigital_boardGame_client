using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

public class BusTransform : MonoBehaviour
{
    private bool should_be_bus = false;

    private void OnEnable()
    {
        UpdateButtonSprite();
    }

    public void UpdateButtonSprite()
    {
        if (GameStateSynchronizer.Instance.Me.is_bus)
        {
            SetButtonSpriteToTurnBackToVehicle();
        }
        else
        {
            SetButtonSpriteToTurnToBus();
        }
    }

    public void ToggleTransform()
    {
        NetworkData.PlayerInput input = new()
        {
            player_id = NetworkData.Instance.UniqueID,
            game_id = GameStateSynchronizer.Instance.LobbyId.Value,
            input_type = NetworkData.PlayerInputType.SetPlayerBusBool.ToString(),
            related_bool = should_be_bus,
        };
        RestAPI.Instance.SendPlayerInput(success =>
        {
            if (should_be_bus)
            {
                SetButtonSpriteToTurnBackToVehicle();
            }
            else
            {
                SetButtonSpriteToTurnToBus();
            }
            UndoSystem.Instance.MovesDone++;
        }, failure =>
        {
            Debug.Log(failure);
        }, input);
    }

    private void SetButtonSpriteToTurnToBus()
    {
        this.gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
    }

    private void SetButtonSpriteToTurnBackToVehicle()
    {
        this.gameObject.GetComponent<SpriteRenderer>().material.color = Color.red;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            should_be_bus = !GameStateSynchronizer.Instance.Me.is_bus;
            ToggleTransform();
        }
    }
}
