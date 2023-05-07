using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusTransform : MonoBehaviour
{
    private bool should_be_bus = false;

    private void Awake()
    {
        if (NetworkData.Instance.Me.Value.is_train)
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
            NetworkData.Player player = NetworkData.Instance.Me.Value;
            player.is_bus = should_be_bus;
            NetworkData.Instance.Me = player;
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
            should_be_bus = !NetworkData.Instance.Me.Value.is_train;
            ToggleTransform();
        }
    }
}
