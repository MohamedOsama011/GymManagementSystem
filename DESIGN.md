# Design System Strategy: Kinetic Precision

## 1. Overview & Creative North Star
**The Creative North Star: "The High-Performance Studio"**

This design system moves away from the static, "boxy" nature of traditional admin dashboards. It treats the interface like a high-end fitness environment: airy, focused, and built for movement. We are building a "Digital High-Performance Studio." 

To break the "template" look, we utilize **Intentional Asymmetry**. Dashboards should not be a repetitive grid of equal boxes. Instead, we use a dominant "Power Column" for high-level metrics, balanced by expansive white space. By utilizing high-contrast typography scales—pairing the geometric authority of `Lexend` with the functional clarity of `Manrope`—the UI feels like an editorial publication for athletes rather than a spreadsheet.

## 2. Colors & Surface Philosophy
The palette centers on a tension between deep, grounded neutrals and a high-energy "Electric Blue."

### The "No-Line" Rule
**Borders are an admission of failure in layout.** In this system, 1px solid borders for sectioning are strictly prohibited. Boundaries are defined solely through background color shifts:
*   **The Canvas:** Uses `surface` (`#fcf9f8`).
*   **The Content Blocks:** Use `surface_container_low` (`#f6f3f2`) to subtly lift sections.
*   **The Focal Points:** Use `surface_container_lowest` (`#ffffff`) for cards that need the most "pop."

### Surface Hierarchy & Nesting
Treat the UI as physical layers. An inner stats widget should use `surface_container_high` (`#ebe7e7`) when sitting on a `surface_container` (`#f0edec`) background. This "tonal nesting" creates depth that feels organic and premium.

### Signature Textures & The "Glass" Rule
*   **Kinetic Gradients:** For primary CTAs and high-impact data viz, use a linear gradient transitioning from `primary` (`#004ae8`) to `primary_container` (`#3766ff`) at a 135-degree angle. This adds "soul" and dimension.
*   **Glassmorphism:** Floating action panels or side navigations should utilize `surface_container_lowest` at 80% opacity with a `24px` backdrop blur. This prevents the UI from feeling "closed off" and maintains the energetic atmosphere.

## 3. Typography
Our typography is a dialogue between "Power" and "Precision."

*   **Display & Headlines (Lexend):** Used for big numbers and section headers. The `display-lg` (3.5rem) should be used for daily revenue or active member counts to create an "Olympic" sense of scale.
*   **Body & Labels (Manrope):** Used for all functional data. `body-md` is our workhorse.
*   **The "Bold Intent" Rule:** Headers (`headline-sm` and up) should always be semi-bold or bold. We want the user to feel the weight of the data. 

## 4. Elevation & Depth
We eschew the "Material 2" shadow-heavy look for **Tonal Layering**.

*   **The Layering Principle:** Depth is achieved by "stacking" tones. A `surface_container_lowest` card placed on a `surface_container_low` background provides enough contrast to signify elevation without a single shadow pixel.
*   **Ambient Shadows:** If a component *must* float (e.g., a modal or a floating action button), use an extra-diffused shadow: `0px 20px 40px rgba(28, 27, 27, 0.06)`. Note the low opacity; it should feel like a soft glow, not a dark smudge.
*   **The "Ghost Border" Fallback:** For high-density data tables where separation is critical, use the `outline_variant` (`#c1c6d7`) at 15% opacity. It should be felt, not seen.

## 5. Components

### Buttons
*   **Primary:** Kinetic gradient (`primary` to `primary_container`), `full` roundedness, white text. No shadow.
*   **Secondary:** `surface_container_highest` background with `on_surface` text. 
*   **Tertiary:** Ghost style. No background; `primary` text. Use for low-emphasis actions like "View All."

### Cards & Structured Tables
*   **The "No-Divider" Rule:** In tables, do not use horizontal lines. Use `16px` of vertical padding between rows and a subtle `surface_container_low` background on `:hover` to indicate focus.
*   **Data Cards:** Use `xl` (0.75rem) corner radius. Elements inside should be aligned to a generous `32px` internal padding to ensure the "clean" aesthetic is maintained.

### Side Navigation
*   **Visual Style:** Use the "Glassmorphism" rule. A `surface_container_lowest` (80% opacity) panel with a blur effect. 
*   **Active State:** Instead of a full-color block, use a `4px` vertical "pill" of `primary` color on the left edge and shift the text to `on_surface` with a bold weight.

### Data Visualization
*   **Line Charts:** Use `primary` for the main trend line with a `primary_container` glow effect (soft shadow in the primary color). 
*   **Bar Charts:** Use `tertiary` (`#9e3d00`) as a high-contrast accent for "Goal Targets" or "Alert States."

## 6. Do's and Don'ts

### Do
*   **Do** use massive white space (up to `64px` or `80px`) between major sections to let the "Electric Blue" breathe.
*   **Do** use `Lexend` for any number that represents a KPI.
*   **Do** use `surface_bright` to highlight the "active" workspace in a multi-panel view.

### Don't
*   **Don't** use black (`#000000`). Use `on_background` (`#1c1b1b`) for text to maintain a professional, charcoal depth.
*   **Don't** use standard `1px` borders. If you feel you need a border, try increasing the spacing or shifting the background tone first.
*   **Don't** crowd the dashboard. If a user has 10 metrics, show the top 3 at `display-md` size and the rest at `title-sm`. Priority is non-negotiable.