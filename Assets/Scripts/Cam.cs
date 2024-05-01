using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    ObjectFader _fader;

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;

            // Luo uusi LayerMask, joka sis‰lt‰‰ LayerMask.IgnoreRaycast -kerroksen
            LayerMask layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");

            // Lis‰‰ LayerMask.IgnoreRaycast -kerros LayerMaskiin
            layerMask |= 1 << LayerMask.NameToLayer("Aiming");

            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            // K‰yt‰ Physics.Raycast metodia ja anna LayerMask parametriksi
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
            {
                if (hit.collider == null) return;

                // Tarkista, osutaanko "Aiming" layeriin
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Aiming"))
                {
                    // Tee t‰‰ll‰ tarvittavat toimenpiteet, kun osutaan "Aiming" layeriin
                    Debug.Log("Osuttiin Aiming layeriin");
                    return;
                }

                if (hit.collider.gameObject == player)
                {
                    if (_fader != null)
                    {
                        _fader.DoFade = false;
                        _fader.ResetFade(); // Lis‰t‰‰n ResetFade-kutsu t‰h‰n
                    }

                    Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
                }
                else
                {
                    _fader = hit.collider.gameObject.GetComponent<ObjectFader>();
                    if (_fader != null) _fader.DoFade = true;

                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                }
            }
        }
    }

}
