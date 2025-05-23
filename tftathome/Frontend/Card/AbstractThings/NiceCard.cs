using System;
using Godot;
using TFTAtHome.Frontend.Singletons;

namespace TFTAtHome.Frontend.Card;

public partial class NiceCard : Node2D {
    NiceCard cardRoot {
        get => this;
    }
    public Vector2 CardPosition {
        get => cardRoot.Position;
        set => cardRoot.Position = value;
    }
    public Vector2 CardGlobalPosition {
        get => cardRoot.GlobalPosition;
        set => cardRoot.GlobalPosition = value;
    }
    public Vector2 CardGlobalScale {
        get => cardRoot.GlobalScale;
        set => cardRoot.GlobalScale = value;
    }
    public Vector2 CardSize {
        get => CardCollision.Shape.GetRect().Size;
    }
    public Vector2 CardSizeGlobal {
        get => new(CardCollision.Shape.GetRect().Size.X * cardRoot.GlobalScale.X, CardCollision.Shape.GetRect().Size.Y * cardRoot.GlobalScale.Y);
    }
    public float Height {
        get => CardCollision.Shape.GetRect().Size.Y * cardRoot.GlobalScale.Y;
    }
    public float Length {
        get => CardCollision.Shape.GetRect().Size.X * cardRoot.GlobalScale.X;
    }
    public CardPlatform Platform { 
        get => cardRoot.GetParent() as CardPlatform;
    }
    public CardLogic CardBody {
        get => cardRoot.GetNode(CardNodeNameSingleton.CardBody) as CardLogic;
    }
    public CollisionShape2D CardCollision {
        get => CardBody.GetNode(CardNodeNameSingleton.CardCollision) as CollisionShape2D;
    }
    public Node2D CardVisuals {
        get => CardBody.GetNode(CardNodeNameSingleton.CardVisuals) as Node2D;
    }
    public ColorRect CardHighlight {
        get => CardVisuals.GetNode("CardHighlight") as ColorRect;
    }
    public ColorRect CardBackground
    {
        get => CardVisuals.GetNode("CardBackground") as ColorRect;
    }
    public ColorRect CharacterNameBackground
    {
        get => CardVisuals.GetNode("CharacterNameBackground") as ColorRect;
    }
    public ColorRect CharacterTitleBackground
    {
        get => CardVisuals.GetNode("CharacterTitleBackground") as ColorRect;
    }
    public Sprite2D CardImg
    {
        get => CardVisuals.GetNode("CardImg") as Sprite2D;
    }
    public RichTextLabel CardName
    {
        get => CardVisuals.GetNode("CardName") as RichTextLabel;
    }
    public RichTextLabel CardTitle
    {
        get => CardVisuals.GetNode("CardTitle") as RichTextLabel;
    }
    public Label Mid_Label_Name
    {
        get => CardVisuals.GetNode("Mid_Label_Name") as Label;
    }
    public Label Early_Label_Name
    {
        get => CardVisuals.GetNode("Early_Label_Name") as Label;
    }
    public Label Late_Label_Name
    {
        get => CardVisuals.GetNode("Late_Label_Name") as Label;
    }
    public Label Early
    {
        get => CardVisuals.GetNode("Early") as Label;
    }
    public Label Mid
    {
        get => CardVisuals.GetNode("Mid") as Label;
    }
    public Label Late
    {
        get => CardVisuals.GetNode("Late") as Label;
    }
    public Label Class_Label_Name
    {
        get => CardVisuals.GetNode("Class_Label_Name") as Label;
    }
    public Label Trait
    {
        get => CardVisuals.GetNode("Trait") as Label;
    }
    public Label CardCost_Label_Name
    {
        get => CardVisuals.GetNode("CardCost_Label_Name") as Label;
    }
    public Label Cost {
        get => CardVisuals.GetNode("Cost") as Label;
    }
}