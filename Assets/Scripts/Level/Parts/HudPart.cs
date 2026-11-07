using PixelAdventure.Game;
using PixelAdventure.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PixelAdventure.Level.Parts
{
    public sealed class HudPart : ILevelPart
    {
        public const string PartId = "HUD";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            var go = LevelContext.CreateChild(parent, "HUD Canvas", Vector3.zero);

            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = RenderOrder.Hud;

            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            var counter = go.AddComponent<ScoreCounter>();

            var score = CreateText(go.transform, "Score Text", 48, TextAnchor.UpperLeft);
            Anchor(score.rectTransform, new Vector2(0f, 1f), new Vector2(40f, -30f), new Vector2(900f, 80f));
            var coinCount = context.Layout.Coins.Length;
            go.AddComponent<ScoreDisplay>().Configure(counter, score, coinCount);

            var overlay = CreateOverlay(go.transform, out var background, out var message);
            go.AddComponent<EndScreen>().Configure(overlay, background, message, counter, coinCount);
        }

        /// <summary>
        /// The HUD survives a level change, so the coin total has to be pushed into the widgets already on screen.
        /// </summary>
        public static void ApplyLayout(LevelContext context)
        {
            var coinCount = context.Layout.Coins.Length;
            var score = context.Find<ScoreDisplay>(PartId);
            if (score != null) score.SetCoinCount(coinCount);

            var endScreen = context.Find<EndScreen>(PartId);
            if (endScreen != null) endScreen.SetCoinCount(coinCount);
        }

        static GameObject CreateOverlay(Transform canvas, out Image background, out Text message)
        {
            var overlay = new GameObject("End Overlay", typeof(RectTransform));
            var rect = (RectTransform)overlay.transform;
            rect.SetParent(canvas, false);
            Stretch(rect);

            background = overlay.AddComponent<Image>();

            message = CreateText(rect, "Message", 96, TextAnchor.MiddleCenter);
            Stretch(message.rectTransform);
            message.supportRichText = true;

            return overlay;
        }

        static Text CreateText(Transform parent, string name, int fontSize, TextAnchor alignment)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);

            var text = go.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;

            var shadow = go.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.6f);
            shadow.effectDistance = new Vector2(3f, -3f);

            return text;
        }

        static void Anchor(RectTransform rect, Vector2 corner, Vector2 position, Vector2 size)
        {
            rect.anchorMin = rect.anchorMax = rect.pivot = corner;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
    }
}
