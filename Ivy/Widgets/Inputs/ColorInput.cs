using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Shared;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

public enum ColorInputVariant
{
    Text,
    Picker,
    TextAndPicker
}

public interface IAnyColorInput : IAnyInput
{
    public string? Placeholder { get; set; }
    public ColorInputVariant Variant { get; set; }
}

public abstract record ColorInputBase : WidgetBase<ColorInputBase>, IAnyColorInput
{
    [Prop] public bool Disabled { get; set; }
    [Prop] public string? Invalid { get; set; }
    [Prop] public string? Placeholder { get; set; }
    [Prop] public bool Nullable { get; set; }
    [Prop] public ColorInputVariant Variant { get; set; } = ColorInputVariant.TextAndPicker;
    [Event] public Action<Event<IAnyInput>>? OnBlur { get; set; }
    public Type[] SupportedStateTypes() => [
        typeof(string),
        typeof(Colors), typeof(Colors?)
        ];
}

public record ColorInput<TColor> : ColorInputBase, IInput<TColor>
{
    public ColorInput(IAnyState state, string? placeholder = null, bool disabled = false, ColorInputVariant variant = ColorInputVariant.TextAndPicker)
        : this(placeholder, disabled, variant)
    {
        var typedState = state.As<TColor>();
        Value = typedState.Value;
        OnChange = e => typedState.Set(e.Value);
    }

    public ColorInput(TColor value, Action<Event<IInput<TColor>, TColor>> onChange, string? placeholder = null, bool disabled = false, ColorInputVariant variant = ColorInputVariant.TextAndPicker)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange;
        Value = value;
    }

    public ColorInput(string? placeholder = null, bool disabled = false, ColorInputVariant variant = ColorInputVariant.TextAndPicker)
    {
        Disabled = disabled;
        Placeholder = placeholder;
        Variant = variant;
    }

    [Prop] public TColor Value { get; } = default!;


    [Event] public Action<Event<IInput<TColor>, TColor>>? OnChange { get; }
}

public record ColorInput : ColorInput<string>
{
    public ColorInput(IAnyState state, string? placeholder = null, bool disabled = false, ColorInputVariant variant = ColorInputVariant.TextAndPicker)
        : base(state, placeholder, disabled, variant)
    {
    }

    public ColorInput(string value, Action<Event<IInput<string>, string>> onChange, string? placeholder = null, bool disabled = false, ColorInputVariant variant = ColorInputVariant.TextAndPicker)
        : base(value, onChange, placeholder, disabled, variant)
    {
    }

    public ColorInput(string? placeholder = null, bool disabled = false, ColorInputVariant variant = ColorInputVariant.TextAndPicker)
        : base(placeholder, disabled, variant)
    {
    }
}

public static class ColorInputExtensions
{
    public static ColorInputBase ToColorInput(this IAnyState state, string? placeholder = null, bool disabled = false, ColorInputVariant variant = ColorInputVariant.TextAndPicker)
    {
        var type = state.GetStateType();
        Type genericType = typeof(ColorInput<>).MakeGenericType(type);
        ColorInputBase input = (ColorInputBase)Activator.CreateInstance(genericType, state, placeholder, disabled, variant)!;
        input.Nullable = type.IsNullableType();
        return input;
    }

    public static ColorInputBase Disabled(this ColorInputBase widget, bool disabled = true)
    {
        return widget with { Disabled = disabled };
    }

    public static ColorInputBase Placeholder(this ColorInputBase widget, string? placeholder)
    {
        return widget with { Placeholder = placeholder };
    }

    public static ColorInputBase Invalid(this ColorInputBase widget, string? invalid)
    {
        return widget with { Invalid = invalid };
    }

    public static ColorInputBase Variant(this ColorInputBase widget, ColorInputVariant variant)
    {
        return widget with { Variant = variant };
    }
}