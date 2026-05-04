---
name: Cosmic Culinary Interface
colors:
  surface: '#121315'
  surface-dim: '#121315'
  surface-bright: '#38393b'
  surface-container-lowest: '#0d0e10'
  surface-container-low: '#1a1c1e'
  surface-container: '#1f2022'
  surface-container-high: '#292a2c'
  surface-container-highest: '#343537'
  on-surface: '#e3e2e4'
  on-surface-variant: '#c3c6ce'
  inverse-surface: '#e3e2e4'
  inverse-on-surface: '#2f3032'
  outline: '#8d9198'
  outline-variant: '#43474d'
  surface-tint: '#b0c9ea'
  primary: '#b0c9ea'
  on-primary: '#18324c'
  primary-container: '#0f2a44'
  on-primary-container: '#7992b1'
  inverse-primary: '#48607d'
  secondary: '#dac49c'
  on-secondary: '#3c2e12'
  secondary-container: '#544526'
  on-secondary-container: '#c8b38b'
  tertiary: '#84dd15'
  on-tertiary: '#1c3700'
  tertiary-container: '#172e00'
  on-tertiary-container: '#5da000'
  error: '#ffb4ab'
  on-error: '#690005'
  error-container: '#93000a'
  on-error-container: '#ffdad6'
  primary-fixed: '#d1e4ff'
  primary-fixed-dim: '#b0c9ea'
  on-primary-fixed: '#001d36'
  on-primary-fixed-variant: '#304864'
  secondary-fixed: '#f7e0b6'
  secondary-fixed-dim: '#dac49c'
  on-secondary-fixed: '#251a02'
  on-secondary-fixed-variant: '#544526'
  tertiary-fixed: '#9ffa3a'
  tertiary-fixed-dim: '#84dd15'
  on-tertiary-fixed: '#0e2000'
  on-tertiary-fixed-variant: '#2c5000'
  background: '#121315'
  on-background: '#e3e2e4'
  surface-variant: '#343537'
typography:
  h1:
    fontFamily: Space Grotesk
    fontSize: 48px
    fontWeight: '700'
    lineHeight: '1.1'
    letterSpacing: 0.05em
  h2:
    fontFamily: Space Grotesk
    fontSize: 32px
    fontWeight: '600'
    lineHeight: '1.2'
    letterSpacing: 0.03em
  h3:
    fontFamily: Space Grotesk
    fontSize: 24px
    fontWeight: '600'
    lineHeight: '1.2'
    letterSpacing: 0.02em
  body-lg:
    fontFamily: Manrope
    fontSize: 18px
    fontWeight: '400'
    lineHeight: '1.6'
    letterSpacing: 0px
  body-md:
    fontFamily: Manrope
    fontSize: 16px
    fontWeight: '400'
    lineHeight: '1.5'
    letterSpacing: 0px
  label-caps:
    fontFamily: Space Grotesk
    fontSize: 12px
    fontWeight: '700'
    lineHeight: '1.0'
    letterSpacing: 0.15em
  stats-num:
    fontFamily: Space Grotesk
    fontSize: 20px
    fontWeight: '500'
    lineHeight: '1.0'
    letterSpacing: 0.02em
spacing:
  base: 8px
  xs: 4px
  sm: 12px
  md: 24px
  lg: 48px
  xl: 64px
  panel-gap: 2px
---

## Brand & Style

This design system establishes a "NASA-punk" aesthetic—a fusion of high-precision aerospace technology and approachable culinary planning. The personality is authoritative and technical yet infused with the wonder of space exploration. It prioritizes a high-utility, cockpit-like experience where meal preparation is treated as mission-critical data.

The visual style utilizes a **Tactile Sci-Fi** approach. It combines sharp, angular "tech panels" for structural data with soft, badge-like circular elements for celebratory actions and status indicators. The interface features isometric vector illustrations with medium-weight strokes and subtle grain textures to provide a physical, "lived-in" feel to the digital environment.

## Colors

The palette is anchored by deep galactic blues, providing a high-contrast foundation for technical data. **Space Blue** serves as the primary atmospheric layer, while **Deep Dark Blue** creates depth in recessed containers and shadows. 

**Gold/Beige** is the primary functional color, used for structural borders and high-priority typography to evoke vintage star charts. **Tech Red** is reserved for critical actions and navigational alerts, while **Neon Green** signifies "Energy" and successful status updates. **Soft White** is used sparingly for highlights and simulated stellar backgrounds.

## Typography

This design system uses a dual-font strategy to balance technical precision with readability. **Space Grotesk** is used for all headers, labels, and numeric data; it should always be set with open tracking (letter-spacing) to mimic radar readouts and aerospace HUDs. Headers should be primarily all-caps.

**Manrope** serves as the secondary typeface for body copy and long-form recipe instructions. It provides a humanistic touch that balances the rigid geometry of the headers, ensuring that complex ingredient lists and preparation steps remain legible during high-activity kitchen tasks.

## Layout & Spacing

The layout follows a **Fixed Tech-Grid** philosophy. Primary content is housed within rigid, structural panels that resemble modular spaceship components. A strict 8px rhythm governs all internal padding.

Layouts should favor a "Dashboard" view rather than a scrolling feed. Content blocks are separated by narrow 2px "panel gaps" that reveal the underlying starry background, creating the illusion of a floating interface. Use isometric perspectives for featured meal cards to break the 2D plane and add a sense of physical hardware.

## Elevation & Depth

Hierarchy is established through **Tonal Layers** and **Bold Outlines** rather than traditional shadows. 
- **Level 0 (Background):** The primary Space Blue gradient with a subtle grain texture and sparse "star" particles.
- **Level 1 (Panels):** Deep Dark Blue surfaces with 1px solid Gold/Beige borders.
- **Level 2 (HUD Overlays):** Semi-transparent (80% opacity) versions of Space Blue with frosted glass blurs and Neon Green accents to indicate active selection.
- **Level 3 (Interaction):** Elements "pop" using Tech Red or Neon Green glows (outer-glow effects) instead of drop shadows to simulate illuminated controls.

## Shapes

This design system utilizes a contrast of "Hard Tech" and "Organic Badge" shapes. 
- **Panels & Containers:** Use sharp, 0px corners. For primary navigation containers, apply a 45-degree "chamfer" or "clipped corner" effect (12px) to the top-right and bottom-left corners.
- **Badges & Status Icons:** Use pill-shaped or circular radii to differentiate them from the structural UI. 
- **Illustrations:** Characters and food items should feature medium-weight outlines and wavy, badge-style container frames to maintain the "cartoon-tech" charm.

## Components

### Buttons
Primary actions use **Tech Red** with sharp corners and all-caps **Space Grotesk** text. Secondary actions use the **Gold/Beige** border-only "ghost" style. All buttons should have a subtle inner-glow on hover.

### Badges & Chips
Status indicators (e.g., "High Protein," "Fuel Low") are rendered as circular or wavy-bordered badges. Use **Neon Green** for positive energy states and **Tech Red** for depletion or warnings.

### Data Panels (Cards)
Meal cards must be isometric. They feature a 1px Gold border and a header strip in a darker shade of Blue. Key metrics (Calories, Prep Time) should be displayed in the **Label-Caps** typography style at the bottom of the card.

### Input Fields
Inputs are recessed containers using the **Deep Dark Blue** color. The cursor and focus state should utilize a "blinking" Neon Green block to reinforce the retro-tech terminal aesthetic.

### Progress Bars
Energy and nutrient tracking use segmented bars (divided into blocks) rather than continuous fills, evoking 8-bit power meters.
