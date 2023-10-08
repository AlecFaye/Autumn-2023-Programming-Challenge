public enum ElementType
{
    Fire,
    Water,
    Earth,
    
    Light,
    Dark
}

public class Element
{
    protected ElementType elementType;
    protected ElementType strongAgainstElement;
    protected ElementType weakAgainstElement;

    public ElementType ElementType => elementType;

    public Element(ElementType elementType, ElementType strongAgainstElement, ElementType weakAgainstElement) 
    { 
        this.elementType = elementType;
        this.strongAgainstElement = strongAgainstElement;
        this.weakAgainstElement = weakAgainstElement;
    }

    public bool IsStrongAgainst(ElementType otherElement)
    {
        return strongAgainstElement == otherElement;
    }

    public bool IsWeakAgainst(ElementType otherElement)
    {
        return weakAgainstElement == otherElement;
    }
}
